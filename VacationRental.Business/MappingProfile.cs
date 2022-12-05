using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Models;
using VacationRental.Infrastructure.Models;

namespace VacationRental.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookingViewModel, Booking>().ReverseMap();
            CreateMap<Booking, BookingBindingModel>();
            CreateMap<RentalViewModel, Rental>().ReverseMap();
            CreateMap<RentalBindingModel, Rental>().ReverseMap();
        }
    }
}
