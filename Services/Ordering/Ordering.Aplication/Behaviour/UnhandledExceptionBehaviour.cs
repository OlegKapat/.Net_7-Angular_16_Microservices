using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Aplication.Behaviour
{
    public class UnhandledExceptionBehaviour<TRequest, TRespons>
        : IPipelineBehavior<TRequest, TRespons>
        where TRequest : IRequest<TRespons>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TRespons> Handle(
            TRequest request,
            RequestHandlerDelegate<TRespons> next,
            CancellationToken cancellationToken
        )
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(
                    ex,
                    $"Unhandled Exception Occurred with Request Name: {requestName}, {request}"
                );
                throw;
            }
        }
    }
}
