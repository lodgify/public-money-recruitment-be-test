using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Infrastructure.Contracts
{
    public interface IBookingRepository : IBaseRepository<BookingViewModel> 
    {
        Task<IEnumerable<BookingViewModel>> GetAllByRentalIdAsync(int rentalId);
    }
}
