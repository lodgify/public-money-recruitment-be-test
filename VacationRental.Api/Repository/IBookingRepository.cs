using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public interface IBookingRepository
    {
        int BookingCount();
        int CreateBooking(BookingViewModel booking);

        bool HasRentalAvailable(int rentalId, DateTime date);

        BookingViewModel GetBooking(int id);

        BookingViewModel[] GetAll();
    }
}