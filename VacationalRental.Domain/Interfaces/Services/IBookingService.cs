using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;

namespace VacationalRental.Domain.Interfaces.Services
{
    public interface IBookingService
    {
        Task<(InsertNewBookingStatus, int)> InsertNewBooking(BookingEntity bookingEntity);
        Task<BookingEntity> GetBookingById(int bookingId);
        Task<bool> BookingExists(int bookingId);
    }
}
