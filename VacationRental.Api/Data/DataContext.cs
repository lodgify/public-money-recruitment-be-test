using System.Collections.Concurrent;
using System.Collections.Generic;
using VacationRental.Api.Data.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Data
{
    public class DataContext : IDataContext
    {
        private int _bookingId;
        private int _rentalId;

        public DataContext()
        {
            Rentals = new ConcurrentDictionary<int, RentalViewModel>();
            Bookings = new ConcurrentDictionary<int, BookingViewModel>();
            _rentalId = 1;
            _bookingId = 1;
        }

        public ConcurrentDictionary<int, RentalViewModel> Rentals { get; }
        public ConcurrentDictionary<int, BookingViewModel> Bookings { get; }
        public int RentalId => _rentalId++;
        public int BookingId => _bookingId++;
    }
}
