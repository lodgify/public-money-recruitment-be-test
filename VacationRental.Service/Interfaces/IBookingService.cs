using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Common.Models;
using VacationRental.Data.Entities;
using VacationRental.Repository.Interfaces;

namespace VacationRental.Service.Interfaces
{
    public interface IBookingService : IBaseService<IBaseRepository<Booking>, BookingViewModel, Booking>
    {
        BookingViewModel SaveBooking(RentalViewModel rental, BookingViewModel model);
        bool CheckAvailability(BookingViewModel model, IEnumerable<BookingViewModel> bookings, RentalViewModel rental);

        bool CheckAvailability(IEnumerable<BookingViewModel> bookings, RentalViewModel rental);
    }
}
