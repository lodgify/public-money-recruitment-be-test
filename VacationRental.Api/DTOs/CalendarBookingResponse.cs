using VacationRental.Domain.Models.Interfaces;

namespace VacationRental.WebAPI.DTOs
{
	public class CalendarBookingResponse : IBookingPeriod
	{
		public int Id { get; set; }

		public int Unit { get; set; }
	}
}
