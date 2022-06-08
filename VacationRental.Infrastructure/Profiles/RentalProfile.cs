using AutoMapper;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;

namespace VacationRental.Infrastructure.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<RentalDto, Rental>()
                .ForMember(x => x.Created, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(x => true));

            CreateMap<RentalParameters, Rental>()
                .ForMember(x => x.Created, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(x => true));
        }
    }
}
