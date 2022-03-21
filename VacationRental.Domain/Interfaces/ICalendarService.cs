using VacationRental.Domain.Models;

namespace VacationRental.Domain.Interfaces
{
	public interface ICalendarService
	{
		public Calendar Get(CalendarRequest calendarRequest);
	}
}
