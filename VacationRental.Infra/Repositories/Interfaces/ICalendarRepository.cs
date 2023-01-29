using System.Threading.Tasks;

namespace VacationRental.Infra.Repositories.Interfaces
{
	public interface ICalendarRepository
	{
		Task GetCalendar();
	}
}
