using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using ApiModels = VacationRental.Api.Models;
using BusinessObjects = VacationRental.BusinessObjects;

namespace VacationRental.Api.Mapping
{
    [ExcludeFromCodeCoverage]
    public class CalendarControllerMappingProfile : Profile
    {
        public CalendarControllerMappingProfile()
        {
            CreateMap<BusinessObjects.Calendar, ApiModels.CalendarViewModel>();
            CreateMap<BusinessObjects.CalendarDate, ApiModels.CalendarDateViewModel>();
            CreateMap<BusinessObjects.CalendarBooking, ApiModels.CalendarBookingViewModel>();
        }
    }
}
