using System.Collections.Generic;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories.ReadOnly
{
    public interface IBookingReadOnlyRepository
    {
        Booking Get(BookingId id);
        IReadOnlyCollection<Booking> GetByRentalId(RentalId rentalId);
    }
}
