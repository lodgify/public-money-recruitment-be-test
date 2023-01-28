using System.Collections;
using System.Collections.Generic;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.PreparationTimes;
using VacationRental.Shared.EntityId;

namespace VacationRental.Domain.Rentals
{
	public class Rental : EntityId
	{
		public Rental()
		{

		}

		public Rental(int units)
		{
			this.Units = units;
			this.Bookings = new List<Booking>();
			this.PreparationTimes = new List<PreparationTime>();
		}

		public int Units { get; set; }

		public virtual ICollection<Booking> Bookings { get; set; }

		public virtual ICollection<PreparationTime> PreparationTimes { get; set; }
	}
}
