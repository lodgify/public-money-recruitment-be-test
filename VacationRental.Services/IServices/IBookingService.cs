using VacationRental.Domain.Models;

namespace VacationRental.Services.IServices
{
    public interface IBookingService
    {
        BookingViewModel Get(int id);
        ResourceIdViewModel Create(BookingBindingModel model);
    }
}
