using LanguageExt;
using System;
using System.Threading.Tasks;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Interfaces
{
    public interface ICalendarService
    {
        Task<Result<CalendarViewModel>> GetRentalCalendarAsync(int rentalId, DateTime start, int nights);
    }
}
