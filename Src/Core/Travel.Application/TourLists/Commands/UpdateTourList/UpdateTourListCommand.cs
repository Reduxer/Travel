using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;
using Travel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Travel.Application.Common.Exceptions;

namespace Travel.Application.TourLists.Commands.UpdateTourList
{
    public class UpdateTourListCommand : IRequest
    {
        public int Id { get; set; }
        public string City { get; set; }

        public string Country { get; set; }

        public string About { get; set; }
    }

    public class UpdateTourListCommandHandler : IRequestHandler<UpdateTourListCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateTourListCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateTourListCommand request, CancellationToken cancellationToken)
        {

            var entity = await _dbContext.TourLists
                .Where(t => t.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(TourList), request.Id);
            }

            entity.City = request.City;
            entity.Country = request.Country;
            entity.About = request.About;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
