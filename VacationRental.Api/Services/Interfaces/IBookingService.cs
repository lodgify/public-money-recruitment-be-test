using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IBookingService
    {
        ResourceIdViewModel Create(BookingBindingModel model);

        BookingViewModel Get(int bookingId);

        IEnumerable<CalendarBookingViewModel> GetBookingsByDate(int rentalId, DateTime date);

        IEnumerable<CalendarPreparationViewModel> GetPreparationsByDate(int rentalId, int preparationTime, DateTime date);
    }
}
