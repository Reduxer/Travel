using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Travel.Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly Stopwatch _timer;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds <= 500) return response;

            var requestName = typeof(TRequest).Name;
            _logger.LogWarning("Travel Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
              requestName, elapsedMilliseconds, request);
            return response;
        }
    }
}
