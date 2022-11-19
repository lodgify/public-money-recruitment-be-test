using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Shared.Infrastructure.Exceptions;

internal class ExceptionCompositionRoot : IExceptionCompositionRoot
{
    private readonly IServiceProvider _serviceProvider;

    public ExceptionCompositionRoot(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ExceptionResponse Map(Exception exception)
    {
        using var scope = _serviceProvider.CreateScope();
        var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToArray();
        var nonDefaultMappers = mappers.Where(x => x is not ExceptionToResponseMapper);
        var result = nonDefaultMappers
            .Select(x => x.Map(exception))
            .SingleOrDefault(x => x is not null);

        if (result is not null)
        {
            return result;
        }

        var defaultMapper = mappers.SingleOrDefault(x => x is ExceptionToResponseMapper);

        return defaultMapper?.Map(exception);
    }
}