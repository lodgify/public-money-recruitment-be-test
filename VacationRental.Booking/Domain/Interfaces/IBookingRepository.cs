
using VacationRental.Domain.Interfaces;

namespace VacationRental.Booking.Domain.Interfaces
{
    public interface IBookingRepository : IAsyncRepository<Domain.Booking>
    {
    }
}