using System.Threading.Tasks;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Models;

namespace VacationRental.Api.Interfaces
{
    public interface ICalendarService
    {
        Task<CalendarViewModel> GetCalendar(ReserveBindingModel model);
    }
}