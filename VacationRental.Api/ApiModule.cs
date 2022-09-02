using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using VacationRental.Api.Infrastructure.Validation;
using VR.Application.Requests.AddRental;
using VR.DataAccess;
using VR.Infrastructure.Modules;

namespace VacationRental.Api
{
    public class ApiModule : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x => x.EnableEndpointRouting = false);

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

            services.AddDbContext<VRContext>();
            services.AddMediatR(typeof(AddRentalRequestHandler).Assembly);
            services.AddValidatorsFromAssembly(typeof(AddRentalValidatorCollection).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}