using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories
{
    public interface IBookingRepository
    {
        Booking Get(BookingId id);
        Booking Add(Booking booking);
    }
}
