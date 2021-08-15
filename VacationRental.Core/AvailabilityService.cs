using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Core
{
    public class AvailabilityService : IAvailabilityService
    {
        public int CheckAvailability(Rental rental, DateTime startDate, int nights, List<Booking> bookings)
        {
            HashSet<int> occupiedUnits = new HashSet<int>();

            for (var i = 0; i < nights; i++)
            {
                var count = 0;

                foreach (var booking in bookings)
                {
                    DateTime bookingEndDate = booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays);
                    DateTime requestEndDate = startDate.AddDays(nights + rental.PreparationTimeInDays);
                    if (booking.RentalId == rental.Id
                        && ((booking.Start <= startDate.Date && bookingEndDate > startDate.Date)
                        || (booking.Start < requestEndDate && bookingEndDate >= requestEndDate)
                        || (booking.Start > startDate && bookingEndDate < requestEndDate)))
                    {
                        count++;
                        occupiedUnits.Add(booking.Unit);
                    }
                }
                if (count >= rental.Units)
                    throw new ApplicationException("Not available");
            }

            return Enumerable.Range(1, rental.Units).Except(occupiedUnits).First();
        }
    }
}
