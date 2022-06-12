using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Models;

namespace VacationRental.Data.Store
{
    public interface IDataContext
    {
        Dictionary<int, RentalViewModel> Rentals { get; set; }
        Dictionary<int, BookingViewModel> Bookings { get; set; }
    }
}
