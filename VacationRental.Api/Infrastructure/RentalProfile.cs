using AutoMapper;
using VacationRental.Core.Domain.Rentals;
using VacationRental.Services.Models;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Api.Infrastructure
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<RentalEntity, RentalViewModel>();
        }
    }
}
