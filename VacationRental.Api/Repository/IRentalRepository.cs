using Models.ViewModels;

namespace VacationRental.Api.Repository;

public interface IRentalRepository
{
    bool IsExists(int id);

    RentalViewModel Get(int id);

    IEnumerable<RentalViewModel> GetAll();

    RentalViewModel Create(int id, RentalViewModel model);
}
