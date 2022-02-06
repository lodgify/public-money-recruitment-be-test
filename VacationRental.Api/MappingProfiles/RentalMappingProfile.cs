using Application.Models.Rental.Responses;
using AutoMapper;
using Domain.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VacationRental.Api.MappingProfiles
{
    public class RentalMappingProfile : Profile
    {
        public RentalMappingProfile()
        {
            CreateMap<Rental, RentalResponse>();
        }
    }
}