using System.Collections.Generic;
using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.DAL
{
    public class DataContext : IDataContext
    {
        private int _bookingId;
        private int _rentalId;

        public DataContext()
        {
            Rentals = new Dictionary<int, RentalViewModel>();
            Bookings = new Dictionary<int, BookingViewModel>();
            _rentalId = 1;
            _bookingId = 1;
        }

        public Dictionary<int, RentalViewModel> Rentals { get; }
        public Dictionary<int, BookingViewModel> Bookings { get; }
        public int RentalId => _rentalId++;
        public int BookingId => _bookingId++;
    }
}
