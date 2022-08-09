using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.DAL.Interfaces
{
    public interface IBookingRepository : IRepository<BookingViewModel>
    {
        IEnumerable<BookingViewModel> GetBookingsByRentalId(int id);
        
    }
}
