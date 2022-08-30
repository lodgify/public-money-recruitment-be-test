using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Contracts.Interfaces;
using Persistence.GenericRepository;

namespace Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection service, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDataBase"))
        {
            service.AddDbContext<ApplicationDbContext>(o=>o.UseInMemoryDatabase("VacationRental"));
        }
        else
        {
            service.AddDbContext<ApplicationDbContext>(o =>
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        
        service.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
      
        return service;
    }
}