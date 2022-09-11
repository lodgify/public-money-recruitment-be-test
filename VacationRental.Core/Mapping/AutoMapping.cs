using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Common.Models;
using VacationRental.Data.Entities;

namespace VacationRental.Core.Mapping
{
	public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			CreateMap<Rental, RentalViewModel>().ReverseMap();
			CreateMap<Booking, BookingViewModel>().ReverseMap();
		}
	}
}
