using System;

namespace VacationRental.Api
{
    public interface IBookingValidator
    {
        bool Validate(DateTime bookingStart, DateTime modelStart, int daysUnavailable);
    }
}