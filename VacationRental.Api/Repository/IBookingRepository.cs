using Models.ViewModels;

namespace VacationRental.Api.Repository;

public interface IBookingRepository
{
    bool IsExists(int id);

    BookingViewModel Get(int id);

    IEnumerable<BookingViewModel> GetAll();

    BookingViewModel Create(int id, BookingViewModel model);
}
