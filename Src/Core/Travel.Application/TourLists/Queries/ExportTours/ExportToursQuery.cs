using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace Travel.Application.TourLists.Queries.ExportTours
{
    public class ExportToursQuery : IRequest<ExportToursVm>
    {
        public int ListId { get; set; }
    }

    public class ExportToursQueryHandler : IRequestHandler<ExportToursQuery, ExportToursVm>
    {
        private readonly IApplicationDbContext _dbContext;

        private readonly IMapper _mapper;

        private readonly ICsvFileBuilder _csvFileBuilder;

        public ExportToursQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ICsvFileBuilder csvFileBuilder)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _csvFileBuilder = csvFileBuilder;
        }

        public async Task<ExportToursVm> Handle(ExportToursQuery request, CancellationToken cancellationToken)
        {
            var vm = new ExportToursVm();

            var records = await _dbContext.TourPackages
                .Where(t => t.ListId == request.ListId)
                .ProjectTo<TourPackageRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            vm.Content = _csvFileBuilder.BuildTourPackageFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = "TourPackages.csv";

            return await Task.FromResult(vm);

        }
    }
}
