using VacationRental.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace VacationRental.WebAPI.DTOs
{
	public class CalendarDateResponse
	{
		public DateTime Date { get; set; }

		public List<IBookingPeriod> Bookings { get; set; }

		public List<IBookingPeriod> PreparationTimes { get; set; }
	}
}
