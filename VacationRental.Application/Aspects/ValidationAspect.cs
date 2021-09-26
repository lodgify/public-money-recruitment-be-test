using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace VacationRental.Application.Aspects
{
    public class ValidationAspect<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationAspect(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await RunValidation(request, cancellationToken);
            return await next();
        }

        private async Task RunValidation(TRequest request, CancellationToken cancellationToken)
        {
            if (_validators.Any() == false) return;

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(result => result.Errors).ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

        }
    }
}
