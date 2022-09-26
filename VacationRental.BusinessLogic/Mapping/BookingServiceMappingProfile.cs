﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using VacationRental.BusinessObjects;
using VacationRental.Repository.Entities;

namespace VacationRental.BusinessLogic.Mapping
{
    [ExcludeFromCodeCoverage]
    public class BookingServiceMappingProfile : Profile
    {
        public BookingServiceMappingProfile()
        {
            CreateMap<BookingEntity, Booking>();
            CreateMap<CreateBooking, BookingEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}