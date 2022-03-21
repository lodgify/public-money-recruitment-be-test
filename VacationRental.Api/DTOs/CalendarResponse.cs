using System.Collections.Generic;

namespace VacationRental.WebAPI.DTOs
{
	public class CalendarResponse
	{
		public int RentalId { get; set; }
		public List<CalendarDateResponse> Dates { get; set; }
	}
}
