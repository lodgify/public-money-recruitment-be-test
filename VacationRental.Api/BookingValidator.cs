using System;

namespace VacationRental.Api
{
    internal class BookingValidator : IBookingValidator
    {
        public bool FoundMatch(DateTime bookingStart, DateTime modelStart, int daysUnavailable)
        {
            var valid = (bookingStart <= modelStart.Date && bookingStart.AddDays(daysUnavailable) > modelStart.Date)
                        || (bookingStart < modelStart.AddDays(daysUnavailable) && bookingStart.AddDays(daysUnavailable) >= modelStart.AddDays(daysUnavailable))
                        || (bookingStart > modelStart && bookingStart.AddDays(daysUnavailable) < modelStart.AddDays(daysUnavailable));

            return valid;
        }
    }
}