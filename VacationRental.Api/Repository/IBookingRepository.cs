using Models.ViewModels;

namespace VacationRental.Api.Repository;

public interface IBookingRepository
{
    Task<bool> IsExists(int id);

    Task<BookingViewModel> Get(int id);

    Task<IEnumerable<BookingViewModel>> GetAll();

    Task<BookingViewModel> Create(int id, BookingViewModel model);
}
