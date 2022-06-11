using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IBookingsService : IGeneralService<BookingViewModel>
    {
        IDictionary<int, BookingViewModel> GetBookingsByRentalId(int rentalId);
        bool CheckIfBookingIsAvailable(BookingBindingModel model, RentalViewModel rental);
        ResourceIdViewModel AddBooking(BookingBindingModel model);
        CalendarViewModel GetBookingCalendar(RentalViewModel rental, DateTime start, int nights);
    }
}
