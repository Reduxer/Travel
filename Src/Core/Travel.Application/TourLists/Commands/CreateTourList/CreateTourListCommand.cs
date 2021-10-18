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

namespace Travel.Application.TourLists.Commands.CreateTourList
{
    public class CreateTourListCommand : IRequest<int>
    {
        public string City { get; set; }

        public string Country { get; set; }

        public string About { get; set; }
    }

    public class CreateTourListCommandHandler : IRequestHandler<CreateTourListCommand, int>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateTourListCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateTourListCommand request, CancellationToken cancellationToken)
        {
            var entity = new TourList()
            {
                City = request.City,
                Country = request.Country,
                About = request.About
            };

            _dbContext.TourLists.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
