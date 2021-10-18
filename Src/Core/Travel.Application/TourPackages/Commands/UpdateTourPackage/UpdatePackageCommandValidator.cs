using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Travel.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Travel.Application.TourPackages.Commands.UpdateTourPackage
{
    public class UpdatePackageCommandValidator : AbstractValidator<UpdateTourPackageCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdatePackageCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.TourPackages.AllAsync(tp => tp.Name != name, cancellationToken);
        }
    }
}
