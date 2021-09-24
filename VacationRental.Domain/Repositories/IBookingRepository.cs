using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories.ReadOnly;

namespace VacationRental.Domain.Repositories
{
    public interface IBookingRepository : IBookingReadOnlyRepository
    {
        Booking Add(Booking booking);
    }
}
