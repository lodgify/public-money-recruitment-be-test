using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Application.Aspects
{
    public sealed class ErrorHandlerAspect<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public ErrorHandlerAspect(ILogger<TRequest> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            // it's forbidden to raise internal errors and passed them to the API level
            // this information might be security-critical
            catch (InfrastructureException ex) 
            {
                _logger.LogError(ex.Message);
                throw new UnhandledInfrastructureException();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                throw new ApplicationException(ex.Message);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex.Message);
                throw new ApplicationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
