using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Storage;

public interface IStateManager
{
    HashSet<RentalViewModel> Rentals { get; }
    HashSet<BookingViewModel> Bookings { get; }
}