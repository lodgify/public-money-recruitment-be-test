using System.Collections.Generic;

namespace VacationRental.Core
{
    public interface IBookingRepository
    {
        List<Booking> GetAll();
        Booking Get(int id);
        int Create(Booking booking);
    }
}
