using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Data.IRepository;
using VacationRental.Domain.Models;

namespace VacationRental.Data.Store
{
    public class DataContext : IDataContext
    {
        public Dictionary<int, RentalViewModel> Rentals { get; set; } = new Dictionary<int, RentalViewModel>();
        public Dictionary<int, BookingViewModel> Bookings { get; set; } = new Dictionary<int, BookingViewModel>();
    }
}
