using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;
using Travel.Application.Common.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.Domain.Entities;

namespace Travel.Application.TourLists.Commands.DeleteTourList
{
    public class DeleteTourListCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteTourListCommandHandler : IRequestHandler<DeleteTourListCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteTourListCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteTourListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.TourLists
                .Where(t => t.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity is null) 
            {
                throw new NotFoundException(nameof(TourList), request.Id);
            }

            _dbContext.TourLists.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
