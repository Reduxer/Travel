using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;


namespace Travel.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; set; }

        public ValidationException() : base("One or more validation failures occured.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            var failureGroups = failures.GroupBy(f => f.PropertyName, f => f.ErrorMessage);

            foreach(var fg in failureGroups)
            {
                var propName = fg.Key;
                var propFailures = fg.ToArray();
                Errors.Add(propName, propFailures);
            }
        }
    }
}
