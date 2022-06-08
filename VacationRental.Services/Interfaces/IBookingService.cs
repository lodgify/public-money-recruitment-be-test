using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;

namespace VacationRental.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(int bookingId);
        Task<BaseEntityDto> AddBookingAsync(BookingParameters parameters);
        Task UpdateBookingAsync(int bookingId, BookingParameters parameters);
        Task DeleteBookingAsync(int bookingId);
    }
}
