using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using ApiModels = VacationRental.Api.Models;
using BusinessObjects = VacationRental.BusinessObjects;

namespace VacationRental.Api.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RentalsControllerMappingProfile : Profile
    {
        public RentalsControllerMappingProfile()
        {
            CreateMap<BusinessObjects.Rental, ApiModels.RentalViewModel>();
            CreateMap<ApiModels.RentalBindingModel, BusinessObjects.CreateRental>();
        }
    }
}
