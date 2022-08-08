using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.DAL.Interfaces
{
    public interface IBookingRepository : IRepository<BookingViewModel>
    {
        IDictionary<int, BookingViewModel> GetAll();
        IEnumerable<BookingViewModel> GetBookingByRentalId(int id);
        
    }
}
