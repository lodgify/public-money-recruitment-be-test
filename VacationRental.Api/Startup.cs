using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Infrastructure.Exceptions;
using VR.Application;
using VR.Infrastructure;
using VR.Infrastructure.Modules;

namespace VacationRental.Api
{
    public class Startup
    {
        private readonly ModulesStartup _modulesStartup;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _modulesStartup = new ModulesStartup();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _modulesStartup.RegisterModule<ApiModule>();
            _modulesStartup.RegisterModule<InfrastructureModule>();
            _modulesStartup.RegisterModule<ApplicationModule>();
            _modulesStartup.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMiddleware<ErrorResponseHandlingMiddleware>(env);

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
        }
    }
}
