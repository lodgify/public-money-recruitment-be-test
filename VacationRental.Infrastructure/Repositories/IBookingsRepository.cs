using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Infrastructure.Models;

namespace VacationRental.Infrastructure.Repositories
{
    public interface IBookingsRepository : IRepository<Booking>
    {
        IEnumerable<Booking> GetConflictingBookings(DateTime start, int nights, int preparationTime, int rentalId);
    }
}
