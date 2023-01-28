using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Application.ViewModels;

namespace VacationRental.Application.Midlewares.Calendar
{
	public class CalendarMiddleware : ICalendarMiddleware
	{
		public CalendarMiddleware()
		{

		}

		public Task GetAvailableCalendar(CalendarInputViewModel input)
		{
			throw new NotImplementedException();
		}
	}
}
