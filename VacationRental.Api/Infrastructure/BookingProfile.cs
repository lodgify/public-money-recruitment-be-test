﻿using AutoMapper;
using VacationRental.Core.Domain.Bookings;
using VacationRental.Services.Models.Booking;

namespace VacationRental.Api.Infrastructure
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingEntity, BookingDto>().ReverseMap();
            CreateMap<CreateBookingRequest, BookingEntity>();
        }
    }
}