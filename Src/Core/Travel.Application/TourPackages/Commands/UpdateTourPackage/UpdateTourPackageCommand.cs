using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Travel.Application.Common.Interfaces;
using Travel.Application.Common.Exceptions;
using Travel.Domain.Entities;

namespace Travel.Application.TourPackages.Commands.UpdateTourPackage
{
    public class UpdateTourPackageCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class UpdatePackageCommandHandler : IRequestHandler<UpdateTourPackageCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdatePackageCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateTourPackageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.TourPackages.FindAsync(request.Id);

            if(entity is null)
            {
                throw new NotFoundException(nameof(TourPackage), request.Id);
            }

            entity.Name = request.Name;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
