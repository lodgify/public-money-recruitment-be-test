using System;
using System.Threading.Tasks;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Infra.Repositories
{
	public class CalendarRepository : ICalendarRepository
	{
		private readonly VacationRentalContext _context;

		public CalendarRepository(VacationRentalContext context)
		{
			this._context = context;
		}
		public Task GetCalendar()
		{
			throw new NotImplementedException();
		}
	}
}
