using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
           var entryAssembly = Assembly.GetExecutingAssembly();
            x.AddConsumers(entryAssembly);

            x.UsingInMemory(
                (context, cfg) =>
                {
                   cfg.ConfigureEndpoints(context);
                });
        });
    }
}