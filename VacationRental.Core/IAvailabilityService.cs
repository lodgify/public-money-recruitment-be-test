using System;
using System.Collections.Generic;

namespace VacationRental.Core
{
    public interface IAvailabilityService
    {
        /// <summary>
        /// Check the availability of a booking and return a Unit if available.
        /// </summary>
        /// <param name="rental">The <see cref="Rental"/> where to book a Unit.</param>
        /// <param name="startDate">The booking start date.</param>
        /// <param name="nights">The duration of booking.</param>
        /// <param name="bookings">A List of <see cref="Booking"/> to check availability against.</param>
        /// <returns>A nondeterministic unit available for the booking requested.</returns>
        int CheckAvailability(Rental rental, DateTime startDate, int nights, List<Booking> bookings);
    }
}
