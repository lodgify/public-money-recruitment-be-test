using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Models.Bookings;

namespace VacationRental.Infrastructure.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public IReadOnlyList<Booking> GetBookingByRentalId(int rentalId)
        {
            return _persistence.Values.Where( b => b.RentalId == rentalId ).OrderBy(b => b.Start).ToList();
        }     
    }
}
