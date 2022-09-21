using AutoMapper;
using VacationRental.Data.Model;

namespace VacationRental.Services.Dto.Mapping;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<BookingDto, Booking>().ReverseMap();
        CreateMap<RentalDto, Rental>().ReverseMap();
    }
}
