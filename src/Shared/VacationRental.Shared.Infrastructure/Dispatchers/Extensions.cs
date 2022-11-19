using Microsoft.Extensions.DependencyInjection;
using VacationRental.Shared.Abstractions.Commands;
using VacationRental.Shared.Abstractions.Dispatchers;
using VacationRental.Shared.Abstractions.Queries;
using VacationRental.Shared.Infrastructure.Commands;
using VacationRental.Shared.Infrastructure.Queries;

namespace VacationRental.Shared.Infrastructure.Dispatchers
{
    public static class Extensions
    {
        public static IServiceCollection AddDispatchers(this IServiceCollection services)
            => services
                .AddSingleton<IDispatcher, InMemoryDispatcher>()
                .AddSingleton<ICommandDispatcher, CommandDispatcher>()
                .AddSingleton<IQueryDispatcher, QueryDispatcher>();
    }
}
