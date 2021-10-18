using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Travel.Application.Common.Interfaces;
using Travel.Application.Common.Exceptions;
using Travel.Domain.Entities;

namespace Travel.Application.TourPackages.Commands.DeleteTourPackage
{
    public class DeleteTourPackageCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteTourPackageCommandHandler : IRequestHandler<DeleteTourPackageCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteTourPackageCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteTourPackageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.TourPackages.FindAsync(request.Id);

            if(entity is null)
            {
                throw new NotFoundException(nameof(TourPackage), request.Id);
            }

            _dbContext.TourPackages.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
