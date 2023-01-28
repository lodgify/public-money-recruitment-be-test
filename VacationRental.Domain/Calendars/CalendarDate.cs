using System;
using System.Collections.Generic;
using VacationRental.Domain.Bookings;
using VacationRental.Shared.EntityId;

namespace VacationRental.Domain.Calendars
{
	public class CalendarDate : EntityId
	{
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public virtual ICollection<Booking> Bookings { get; set; }
	}
}
