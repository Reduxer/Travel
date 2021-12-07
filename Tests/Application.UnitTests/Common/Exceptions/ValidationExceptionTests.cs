using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Application.UnitTests.Common.Exceptions
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void DefaultConstructorCreatesAnEmptyErrorDictionary()
        {
            var exception = new ValidationException();
            exception.Errors.Keys.Should().BeEquivalentTo(Array.Empty<string>());
        }

        [Fact]
        public void SingleValidationFailure()
        {
            var failures = new List<ValidationFailure>()
            {
                new("Mobile", "Mobile is required.")
            };

            var exception = new ValidationException(failures);
            var errors = exception.Errors;

            errors.Keys.Should().BeEquivalentTo("Mobile");
            errors["Mobile"].Should().BeEquivalentTo("Mobile is required.");
        }
    }
}
