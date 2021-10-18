using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Travel.Application.TourPackages.Commands.UpdateTourPackageDetail
{
    public class UpdateTourPackageDetailCommandValidator : AbstractValidator<UpdateTourPackageDetailCommand>
    {
        public UpdateTourPackageDetailCommandValidator()
        {
            RuleFor(v => v.ListId)
                .NotEmpty().WithMessage("ListId is required");
            RuleFor(v => v.WhatToExpect)
              .NotEmpty().WithMessage("WhatToExpect is required");
            RuleFor(v => v.MapLocation)
              .NotEmpty().WithMessage("MapLocation is required");
            RuleFor(v => v.Price)
              .NotEmpty().WithMessage("Price is required");
            RuleFor(v => v.Duration)
              .NotEmpty().WithMessage("Duration is required");
            RuleFor(v => v.InstantConfirmation)
              .NotEmpty().WithMessage("InstantConfirmation is required");
            RuleFor(v => v.Currency)
              .NotEmpty().WithMessage("Currency is required");
        }
    }
}
