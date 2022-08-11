using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public interface IBookingRepository
    {
        int BookingCount();
        int CreateBooking(BookingViewModel booking);

        bool HasRentalBooking(int rentalId, DateTime date ,DateTime endDate,int preparationTimesDays);

        BookingViewModel GetBooking(int id);

        BookingViewModel GetBooking(int rentalId, DateTime date);

        List<DateTime> GetPreparationTimes(int rentalId, DateTime startDate,DateTime endDate, int preparationTime);


    }
}