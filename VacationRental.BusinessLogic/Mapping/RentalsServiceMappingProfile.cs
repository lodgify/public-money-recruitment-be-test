using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using VacationRental.BusinessObjects;
using VacationRental.Repository.Entities;

namespace VacationRental.BusinessLogic.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RentalsServiceMappingProfile : Profile
    {
        public RentalsServiceMappingProfile()
        {
            CreateMap<RentalEntity, Rental>();
            CreateMap<CreateRental, RentalEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
