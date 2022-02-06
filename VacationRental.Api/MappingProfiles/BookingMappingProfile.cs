using Application.Models.Booking.Responses;
using AutoMapper;
using Domain.DAL.Models;

namespace VacationRental.Api.MappingProfiles
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            CreateMap<Booking, BookingResponse>();
        }
    }
}