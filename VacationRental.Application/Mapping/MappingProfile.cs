using AutoMapper;
using VacationRental.Domain.Messages.Bookings;
using VacationRental.Domain.Messages.Rentals;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Booking, BookingDto>();
            CreateMap<Rental, RentalDto>();
        }
    }
}
