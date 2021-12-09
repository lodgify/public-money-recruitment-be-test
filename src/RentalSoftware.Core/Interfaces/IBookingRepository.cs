using RentalSoftware.Core.Entities;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Interfaces
{
    public interface IBookingRepository : IRepositoryBase<Booking>
    {
        Task<Booking> GetByIdAsync(int Id);
    }
}
