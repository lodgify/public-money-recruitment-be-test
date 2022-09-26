using System.Collections.Generic;
using VacationRental.Repository.Entities;

namespace VacationRental.Repository.Repositories.Interfaces
{
    public interface IBookingsRepository
    {
        BookingEntity GetBookingEntity(int bookingId);
        List<BookingEntity> GetBookingEntities();
        int CreateBookingEntity(BookingEntity bookingEntity);
    }
}
