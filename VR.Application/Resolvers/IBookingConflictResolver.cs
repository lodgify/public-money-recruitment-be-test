using System.Collections.Generic;
using VR.Domain.Models;

namespace VR.Application.Resolvers
{
    public interface IBookingConflictResolver
    {
        IEnumerable<Booking> GetCrossBookedUnits(Rental rental, Booking currentBooking, IEnumerable<Booking> rentalBookings);

        int GetAvailableUnit(Rental rental, IEnumerable<Booking> rentalBookings);

        bool HasBookingConflicts(Rental rental, IEnumerable<Booking> rentalBookings);
    }
}
