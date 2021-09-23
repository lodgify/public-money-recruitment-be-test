using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories
{
    public interface IBookingRepository
    {
        Booking Get(BookingId id);
        IReadOnlyCollection<Booking> GetByRentalId(RentalId rentalId);
        Booking Add(Booking booking);
    }
}
