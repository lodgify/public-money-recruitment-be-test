using System.Collections.Generic;
using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.DAL
{
    public class DataContext : IDataContext
    {
        private readonly Dictionary<int, RentalViewModel> _rentals;
        private readonly Dictionary<int, BookingViewModel> _bookings;

        public DataContext()
        {
            _rentals = new Dictionary<int, RentalViewModel>();
            _bookings = new Dictionary<int, BookingViewModel>();
        }

        public Dictionary<int, RentalViewModel> Rentals => _rentals;
        public Dictionary<int, BookingViewModel> Bookings => _bookings;
    }
}
