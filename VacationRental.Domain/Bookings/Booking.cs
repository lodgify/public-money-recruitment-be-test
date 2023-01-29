using System;
using VacationRental.Domain.Calendars;
using VacationRental.Domain.Rentals;
using VacationRental.Shared.EntityId;

namespace VacationRental.Domain.Bookings
{
	public class Booking : EntityId
	{
		public Booking()
		{

		}

		public Booking(CalendarDate calendarDate, int rentalId, int nights, int unity)
		{
			this.CalendarDate = calendarDate;
			this.RentalId = rentalId;
			this.Nights = nights;
			this.Unity = unity;
		}

		public virtual CalendarDate CalendarDate { get; set; }

		public virtual Rental Rental { get; set; }

		public int RentalId { get; set; }

		public int Nights { get; set; }

		public int Unity { get; set; }
	}
}
