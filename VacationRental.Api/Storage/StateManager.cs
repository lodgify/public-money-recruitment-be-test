using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Storage;

public sealed class StateManager : IStateManager
{
    public HashSet<RentalViewModel> Rentals { get; } = new();
    public HashSet<BookingViewModel> Bookings { get; } = new();
}