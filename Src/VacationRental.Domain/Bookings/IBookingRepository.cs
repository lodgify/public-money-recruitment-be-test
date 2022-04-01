using System.Collections.Generic;

namespace VacationRental.Domain.Bookings
{
    public interface IBookingRepository
    {
        int Add(BookingModel model);
        BookingModel Get(int id);
        IEnumerable<BookingModel> GetByRentalId(int id);
    }
}
