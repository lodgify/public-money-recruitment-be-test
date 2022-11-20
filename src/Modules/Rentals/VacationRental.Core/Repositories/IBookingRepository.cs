using System.Threading.Tasks;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Repositories
{
    internal interface IBookingRepository
    {
        Task<Booking> GetAsync(int id);
        Task AddAsync(Booking booking);
    }
}
