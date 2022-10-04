using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using VacationRental.Domain;
using Xunit;

namespace VacationRental.Api.IntegrationTests
{
    [CollectionDefinition("Integration")]
    public sealed class VacationRentalWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<VacationRentalDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<VacationRentalDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Test");
                });
            });
        }
    }
}
