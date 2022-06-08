using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;

namespace VacationRental.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDto> GetBookingByIdAsync(int bookingId);
        Task<BaseEntityDto> AddBookingAsync(BookingParameters parameters);
    }
}
