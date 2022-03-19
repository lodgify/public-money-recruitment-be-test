using VacationRental.Domain.Models.Interfaces;

namespace VacationRental.WebAPI.DTOs
{
	public class PreparationTimeResponse: IBookingPeriod
	{
		public int Unit { get; set; }
	}
}
