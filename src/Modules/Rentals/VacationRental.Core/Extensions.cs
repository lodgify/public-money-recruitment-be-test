using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VacationRental.Api")]
[assembly: InternalsVisibleTo("VacationRental.Application")]
[assembly: InternalsVisibleTo("VacationRental.Infrastructure")]
[assembly: InternalsVisibleTo("VacationRental.Tests.Unit")]
[assembly: InternalsVisibleTo("VacationRental.Tests.Integration")]
[assembly: InternalsVisibleTo("VacationRental.Tests.EndToEnd")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace VacationRental.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            return services;
        }
    }
}
