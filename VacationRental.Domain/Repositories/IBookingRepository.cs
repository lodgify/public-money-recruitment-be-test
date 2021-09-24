using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories.ReadOnly;

namespace VacationRental.Domain.Repositories
{
    public interface IBookingRepository : IBookingReadOnlyRepository
    {
        ValueTask<Booking> Add(Booking booking);
    }
}
