using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Travel.Application.Dtos.Tour;
using Travel.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Travel.Application.TourPackages.Queries.GetTourPackages
{
    public class GetTourPackagesQuery : IRequest<TourPackagesVm>
    {
        public int ListId { get; set; }
    }

    public class GetTourPackagesQueryHandler : IRequestHandler<GetTourPackagesQuery, TourPackagesVm>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetTourPackagesQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<TourPackagesVm> Handle(GetTourPackagesQuery request, CancellationToken cancellationToken)
        {
            var tourPackages = await _applicationDbContext.TourPackages
                .Where(tp => tp.ListId == request.ListId)
                .OrderBy(tp => tp.Name)
                .ProjectTo<TourPackageDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new TourPackagesVm()
            {
                Lists = tourPackages,
            };
        }
    }
}
