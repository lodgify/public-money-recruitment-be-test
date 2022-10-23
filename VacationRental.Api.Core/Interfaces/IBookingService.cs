using LanguageExt;
using System.Threading.Tasks;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Core.Interfaces
{
    public interface IBookingService
    {
        Task<Result<BookingViewModel>> GetBookingByIdAsync(int bookingId);
        Task<Result<ResourceIdViewModel>> InsertNewBookingAsync(BookingBindingModel booking);
    }
}
