using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Travel.Application.TourPackages.Queries.GetTourPackages
{
    public class GetTourPackagesQueryValidator : AbstractValidator<GetTourPackagesQuery>
    {
        public GetTourPackagesQueryValidator()
        {
            RuleFor(x => x.ListId)
                .NotNull()
                .NotEmpty().WithMessage("ListId is required");
        }
    }
}
