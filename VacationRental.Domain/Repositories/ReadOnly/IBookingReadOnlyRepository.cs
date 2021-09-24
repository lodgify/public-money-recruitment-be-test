using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories.ReadOnly
{
    public interface IBookingReadOnlyRepository
    {
        ValueTask<Booking> Get(BookingId id);
        ValueTask<IReadOnlyCollection<Booking>> GetByRentalId(RentalId rentalId);
    }
}
