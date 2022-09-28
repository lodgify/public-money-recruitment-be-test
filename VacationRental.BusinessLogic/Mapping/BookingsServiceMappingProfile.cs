using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using VacationRental.BusinessObjects;
using VacationRental.Repository.Entities;

namespace VacationRental.BusinessLogic.Mapping
{
    [ExcludeFromCodeCoverage]
    public class BookingsServiceMappingProfile : Profile
    {
        public BookingsServiceMappingProfile()
        {
            CreateMap<BookingEntity, Booking>();
            CreateMap<CreateBooking, BookingEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
