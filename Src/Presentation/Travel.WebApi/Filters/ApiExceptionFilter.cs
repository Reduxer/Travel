using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Travel.Application.Common.Exceptions;

namespace Travel.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Dictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilter()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
            };
        }

        public override void OnException(ExceptionContext exceptionContext)
        {
            HandleException(exceptionContext);
            base.OnException(exceptionContext);
        }

        private void HandleException(ExceptionContext exceptionContext)
        {
            Type t = exceptionContext.Exception.GetType();

            if (_exceptionHandlers.ContainsKey(t))
            {
                _exceptionHandlers[t].Invoke(exceptionContext);
                return;
            }

            HandleUnknownException(exceptionContext);
        }

        private static void HandleUnknownException(ExceptionContext exceptionContext)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            exceptionContext.Result = new BadRequestObjectResult(details);
            exceptionContext.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext exceptionContext)
        {
            var exception = exceptionContext.Exception as ValidationException;

            var details = new ValidationProblemDetails(exception.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            exceptionContext.Result = new BadRequestObjectResult(details);

            exceptionContext.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext exceptionContext)
        {
            var ex = exceptionContext.Exception as NotFoundException;

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = ex.Message
            };

            exceptionContext.Result = new NotFoundObjectResult(details);
            exceptionContext.ExceptionHandled = true;
        }
    }
}
