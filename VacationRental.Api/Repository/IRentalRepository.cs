using Models.ViewModels;

namespace VacationRental.Api.Repository;

public interface IRentalRepository
{
    Task<bool> IsExists(int id);

    Task<RentalViewModel> Get(int id);

    Task<IEnumerable<RentalViewModel>> GetAll();

    Task<RentalViewModel> Create(int id, RentalViewModel model);
}
