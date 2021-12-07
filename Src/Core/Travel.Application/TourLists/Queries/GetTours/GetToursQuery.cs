using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Linq;
using Travel.Application.Common.Interfaces;
using AutoMapper.QueryableExtensions;
using Travel.Application.Dtos.Tour;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Travel.Application.TourLists.Queries.GetTours
{
    public class GetToursQuery : IRequest<ToursVm> 
    {
        public bool NoCache { get; set; } = false;
    }

    public class GetToursQueryHandler : IRequestHandler<GetToursQuery, ToursVm>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public GetToursQueryHandler(IApplicationDbContext dbContext, IMapper mapper, IDistributedCache distributedCache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        public async Task<ToursVm> Handle(GetToursQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "GetTours";
            ToursVm toursVm = null;
            string serializedTourList = null;

            if (request.NoCache)
            {
                toursVm = new ToursVm()
                {
                    Lists = await _dbContext.TourLists
                    .ProjectTo<TourListDto>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.City)
                    .ToListAsync(cancellationToken)
                };
            }
            else
            {
                var cachedTourLists = await _distributedCache.GetAsync(cacheKey, cancellationToken);

                if (cachedTourLists is null)
                {
                    toursVm = new ToursVm()
                    {
                        Lists = await _dbContext.TourLists
                        .ProjectTo<TourListDto>(_mapper.ConfigurationProvider)
                        .OrderBy(t => t.City)
                        .ToListAsync(cancellationToken)
                    };

                    serializedTourList = JsonSerializer.Serialize(toursVm);
                    cachedTourLists = Encoding.UTF8.GetBytes(serializedTourList);

                    var cachingOptions = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                    await _distributedCache.SetAsync(cacheKey, cachedTourLists, cachingOptions);

                    return toursVm;
                }

                serializedTourList = Encoding.UTF8.GetString(cachedTourLists);
                toursVm = JsonSerializer.Deserialize<ToursVm>(serializedTourList);
            }
            
            return toursVm;
        }
    }
}
