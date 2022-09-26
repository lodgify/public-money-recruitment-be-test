using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using ApiModels = VacationRental.Api.Models;
using BusinessObjects = VacationRental.BusinessObjects;

namespace VacationRental.Api.Mapping
{
    [ExcludeFromCodeCoverage]
    public class BookingsControllerMappingProfile : Profile
    {
        public BookingsControllerMappingProfile()
        {
            CreateMap<BusinessObjects.Booking, ApiModels.BookingViewModel>();
            CreateMap<ApiModels.BookingBindingModel, BusinessObjects.CreateBooking>();
        }
    }
}
