using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentValidation;
using Travel.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Travel.Application.TourPackages.Commands.CreateTourPackage
{
    public class CreateTourPackageCommandValidator : AbstractValidator<CreateTourPackageCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateTourPackageCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(v => v.Name).
                NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
        }

        public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.TourPackages.AllAsync(tp => tp.Name != name);
        }
    }
}
