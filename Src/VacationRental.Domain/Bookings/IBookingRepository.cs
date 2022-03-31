using System.Collections.Generic;

namespace VacationRental.Domain.Bookings
{
    public interface IBookingRepository
    {
        int Save(BookingModel model);
        BookingModel Get(int id);

        int GetLastId();

        ICollection<BookingModel> GetAll();
        
        IEnumerable<BookingModel> GetByRentalId(int id);

    }
}