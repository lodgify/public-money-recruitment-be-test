using System;
using VacationRental.Domain.Rentals;
using VacationRental.Shared.EntityId;

namespace VacationRental.Domain.PreparationTimes
{
	public class PreparationTime : EntityId
	{
		public PreparationTime()
		{

		}

		public PreparationTime(int unity, int rentalId)
		{
			this.Unity = unity;
			this.RentalId = rentalId;
		}

		public int Unity { get; set; }

		public DateTime DateOfPreparation { get; set; }

		public virtual Rental Rental { get; set; }

		public int RentalId { get; set; }
	}
}
