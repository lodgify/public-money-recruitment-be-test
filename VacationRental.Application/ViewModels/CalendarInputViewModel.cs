using System;
using System.Collections.Generic;

namespace VacationRental.Application.ViewModels
{
	public class CalendarInputViewModel
	{
		public int RentalId { get; set; }

		public int Nights { get; set; }

		public DateTime DateStart { get; set; }

		public List<BookingInputViewModel> Bookings { get; set; }

		public List<PreparationTimeInputViewModel> PreparationTimes { get; set; }
	}
}
