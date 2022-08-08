using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.DAL.Interfaces
{
    public interface IDataContext
    {
        Dictionary<int, RentalViewModel> Rentals { get; }
        Dictionary<int, BookingViewModel> Bookings { get; }
    }
}
