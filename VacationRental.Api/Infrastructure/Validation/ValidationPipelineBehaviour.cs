using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VacationRental.Api.Infrastructure.Validation
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators != null)
            {
                var validationResults = await Task.WhenAll(
                    _validators
                        .Where(v => v != null)
                        .Select(v => v.ValidateAsync(request, cancellationToken)));

                var errors = validationResults
                    .SelectMany(v => v.Errors)
                    .ToList();

                if (errors.Count > 0)
                {
                    throw new ValidationException(
                        message: "Validation errors",
                        errors: errors,
                        appendDefaultMessage: true);
                }
            }

            return await next();
        }
    }
}
