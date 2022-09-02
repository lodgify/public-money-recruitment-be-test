using System;
using System.Collections.Generic;
using System.Linq;
using VR.Domain.Models;
using VR.Infrastructure.Exceptions;

namespace VR.Application.Resolvers
{
    public class BookingConflictResolver : IBookingConflictResolver
    {
        public IEnumerable<Booking> GetCrossBookedUnits(Rental rental, Booking currentBooking, IEnumerable<Booking> rentalBookings)
        {
            return rentalBookings.Where(
                b => b.Start <= currentBooking.Start && b.End.AddDays(rental.PreparationTimeInDays) > currentBooking.Start ||
                b.Start < currentBooking.End.AddDays(rental.PreparationTimeInDays) && b.End.AddDays(rental.PreparationTimeInDays) > currentBooking.Start);
        }

        public int GetAvailableUnit(Rental rental, IEnumerable<Booking> rentalCrossedBookings)
        {
            for (int i = 1; i <= rental.Units; i++)
            {
                if (!rentalCrossedBookings.Any(x => x.Unit == i))
                    return i;
            }

            throw new BookingConflictsException("No available unit", "No available Unit in rental");
        }

        public bool HasBookingConflicts(Rental rental, IEnumerable<Booking> rentalBookings)
        {
            if (!rentalBookings.Any())
                return false;

            var startDate = rentalBookings.Min(p => p.Start);
            var endDate = rentalBookings.Max(p => p.End).AddDays(rental.PreparationTimeInDays);

            //startDate = startDate > DateTime.Now ? startDate : DateTime.Now; 
            //Uncomment in case if we need to check crossbookgins since current date!!!

            for (var i = 0; startDate.AddDays(i) <= endDate; i++)
            {
                var date = startDate.AddDays(i);

                var activeDailyBookingCount = rentalBookings.Count(
                    p => p.Start <= date &&
                    p.End.AddDays(rental.PreparationTimeInDays) > date);

                if (activeDailyBookingCount > rental.Units)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
