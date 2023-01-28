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

		public PreparationTime(int unity, Rental rental)
		{
			this.Unity = unity;
			this.Rental = rental;
		}

		public int Unity { get; set; }

		public DateTime DateOfPreparation { get; set; }

		public virtual Rental Rental { get; set; }
	}
}
