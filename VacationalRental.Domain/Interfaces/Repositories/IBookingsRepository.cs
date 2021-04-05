using System.Collections.Generic;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;

namespace VacationalRental.Domain.Interfaces.Repositories
{
    public interface IBookingsRepository
    {
        Task<IEnumerable<BookingEntity>> GetBookings();

        Task<BookingEntity> GetBookingById(int bookingId);

        Task<int> InsertBooking(BookingEntity bookingEntity);

        Task<bool> BookingExists(int bookingId);
    }
}
