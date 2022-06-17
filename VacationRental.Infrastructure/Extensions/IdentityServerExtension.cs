using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.DataAccess.Contexts;
using VacationRental.Models.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;

namespace VacationRental.Infrastructure.Extensions
{
    public static class IdentityServerExtension
    {
        public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            const string apiResourceName = "api";
            const string identityServerSectionName = "IdentityServer";
            const string identityServerConnectionParamName = "IdentityServerConnection";

            var migrationsAssembly = typeof(IdentityServerExtension).GetTypeInfo().Assembly.GetName().Name;

            var identityServerConnection = configuration.GetConnectionString(identityServerConnectionParamName);

            services.AddDbContext<VacationRentalPersistedGrantDbContext>(options => options.UseSqlServer(identityServerConnection,
                                                                                    options => options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));

            services.AddDbContext<VacationRentalConfigurationDbContext>(options => options.UseSqlServer(identityServerConnection,
                                                                                   options => options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));

            services.AddDbContext<VacationRentalIdentityServerDbContext>(options => options.UseSqlServer(identityServerConnection,
                                                                                    options => options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));

            services.AddIdentityServer()
                    .AddConfigurationStore(options => options.ConfigureDbContext = builder => builder.UseSqlServer(identityServerConnection, sql => sql.MigrationsAssembly(migrationsAssembly)))
                    .AddOperationalStore(options => options.ConfigureDbContext = builder => builder.UseSqlServer(identityServerConnection, sql => sql.MigrationsAssembly(migrationsAssembly)))
                    .AddDeveloperSigningCredential();

            var identityServerConfiguration = configuration.GetSection(identityServerSectionName).Get<IdentityServerConfiguration>();
            services.Configure<IdentityServerConfiguration>(configure => configure.ServiceUrl = identityServerConfiguration.ServiceUrl);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = identityServerConfiguration.ServiceUrl;
                options.Audience = apiResourceName;
                options.RequireHttpsMetadata = false;
            });
        }
    }
}
