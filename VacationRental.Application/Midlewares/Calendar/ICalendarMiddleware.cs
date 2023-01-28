using System.Threading.Tasks;
using VacationRental.Application.ViewModels;

namespace VacationRental.Application.Midlewares.Calendar
{
	public interface ICalendarMiddleware
	{
		Task GetAvailableCalendar(CalendarInputViewModel input);
	}
}
