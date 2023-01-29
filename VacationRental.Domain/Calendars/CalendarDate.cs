using System;
using System.Collections.Generic;
using VacationRental.Domain.Bookings;
using VacationRental.Shared.EntityId;

namespace VacationRental.Domain.Calendars
{
	public class CalendarDate : EntityId
	{
		public CalendarDate()
		{

		}

		public CalendarDate(DateTime startDate, DateTime endDate)
		{
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.Bookings = new List<Booking>();
		}

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public virtual ICollection<Booking> Bookings { get; set; }
	}
}
