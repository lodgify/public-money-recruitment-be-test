using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface ICalenderService
    {
        CalendarViewModel CalenderGetService(int rentalId, DateTime start, int nights,
                        IDictionary<int, RentalViewModel> _rentals, IDictionary<int, BookingViewModel> _bookings);

    }
}
