using System;
using VacationRental.Domain.Rentals;
using VacationRental.Shared.EntityId;

namespace VacationRental.Domain.PreparationTimes
{
	public class PreparationTime : EntityId
	{
		public DateTime DateOfPreparation { get; set; }

		public  virtual Rental Rental { get; set; }
	}
}
