using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Models;

namespace VacationalRental.Domain.Interfaces.Services
{
    public interface ICalendarService
    {
        Task<CalendarModel> GetRentalCalendarByNights(int rentalId, DateTime start, int nights);
    }
}
