using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Domain.VacationRental.Interfaces.Repositories
{
    public interface IRentalsRepository
    {
        Task<RentalViewModel> Get(int rentalId);
        Task<List<RentalViewModel>> Get();
        Task<int?> GetLastId();
        Task<ResourceIdViewModel> Post(RentalViewModel rentalModel);
        Task<ResourceIdViewModel> Put(int rentalId, RentalBindingModel rentalModel);
    }
}
