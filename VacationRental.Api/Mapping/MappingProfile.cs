using AutoMapper;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Models;

namespace VacationRental.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookingBindingModel, BookingViewModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom((_, _, _, context) => context.Items["Id"]));

            CreateMap<RentalBindingModel, RentalViewModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom((_, _, _, context) => context.Items["Id"]));     
        }
    }
}