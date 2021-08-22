using System;

namespace VacationRental.Api
{
    public interface IBookingValidator
    {
        bool FoundMatch(DateTime bookingStart, DateTime modelStart, int daysUnavailable);
    }
}