using AutoMapper;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;

namespace VacationRental.Infrastructure.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>().ReverseMap();

            CreateMap<BookingParameters, Booking>()
                .ForMember(x => x.Start, opt => opt.MapFrom(x => x.Start.HasValue ? x.Start.Value.Date : DateTime.UtcNow))
                .ForMember(x => x.Created, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(x => true))
                .ReverseMap();
        }
    }
}
