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

namespace Travel.Application.TourLists.Queries.GetTours
{
    public class GetToursQuery : IRequest<ToursVm> {  }

    public class GetToursQueryHandler : IRequestHandler<GetToursQuery, ToursVm>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetToursQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ToursVm> Handle(GetToursQuery request, CancellationToken cancellationToken)
        {
            return new ToursVm()
            {
                Lists = await _dbContext.TourLists
                    .ProjectTo<TourListDto>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.City)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
