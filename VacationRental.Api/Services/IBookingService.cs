using System.Threading.Tasks;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IBookingService
    {
        BookingViewModel GetBooking(int id);
        
        ResourceIdViewModel Create(BookingBindingModel model);
    }
}