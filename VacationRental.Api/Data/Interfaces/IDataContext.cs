using System.Collections.Concurrent;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Data.Interfaces
{
    public interface IDataContext
    {
        ConcurrentDictionary<int, RentalViewModel> Rentals { get; }
        ConcurrentDictionary<int, BookingViewModel> Bookings { get; }

        int RentalId { get; }
        int BookingId { get; }
    }
}
