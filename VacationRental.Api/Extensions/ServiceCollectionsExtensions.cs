using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Mapping;
using VacationRental.BusinessLogic.Extensions;
using VacationRental.BusinessLogic.Mapping;
using VacationRental.BusinessLogic.Services.Validators;
using VacationRental.Repository.Extensions;

namespace VacationRental.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionsExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddBusinessLogicServices();
            services.AddRepositoryServices();

            RegisterSolutionAutoMapperProfiles(services);
            RegisterSolutionFluentValidators(services);
        }

        private static void RegisterSolutionAutoMapperProfiles(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BookingsControllerMappingProfile), typeof(RentalsServiceMappingProfile));
        }

        private static void RegisterSolutionFluentValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(CreateBookingValidator));
        }
    }
}
