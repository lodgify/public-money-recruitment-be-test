using System.Collections.Generic;
using VacationRental.Domain.Models.Bookings;

namespace VacationRental.Application.Contracts.Persistence
{
    public interface IBookingRepository : IRepository<Booking>
    {
        IReadOnlyList<Booking> GetBookingByRentalId(int rentalId);
    }
}
