using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Travel.Domain.Enums;
using Travel.Application.Common.Interfaces;
using Travel.Application.Common.Exceptions;
using Travel.Domain.Entities;

namespace Travel.Application.TourPackages.Commands.UpdateTourPackageDetail
{
    public class UpdateTourPackageDetailCommand : IRequest
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string WhatToExpect { get; set; }
        public string MapLocation { get; set; }
        public float Price { get; set; }
        public int Duration { get; set; }
        public bool InstantConfirmation { get; set; }
        public Currency Currency { get; set; }
    }

    public class UpdateTourPackageDetailCommandHandler : IRequestHandler<UpdateTourPackageDetailCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateTourPackageDetailCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateTourPackageDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.TourPackages.FindAsync(request.Id);

            if(entity is null)
            {
                throw new NotFoundException(nameof(TourPackage), request.Id);
            }

            entity.ListId = request.ListId;
            entity.WhatToExpect = request.WhatToExpect;
            entity.MapLocation = request.MapLocation;
            entity.Price = request.Price;
            entity.Duration = request.Duration;
            entity.InstantConfirmation = request.InstantConfirmation;
            entity.Currency = request.Currency;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
