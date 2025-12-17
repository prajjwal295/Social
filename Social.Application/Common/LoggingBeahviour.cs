using Azure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Common
{
    public class LoggingBeahviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly ILogger<LoggingBeahviour<TRequest, TResponse>> _logger;

        public LoggingBeahviour(ILogger<LoggingBeahviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling {RequestName} with {@Request}", typeof(TRequest).Name, request);

            var response = await next();

            _logger.LogInformation("Handled {RequestName} with {@Response}", typeof(TRequest).Name, response);

            return response;
        }
    }
}
