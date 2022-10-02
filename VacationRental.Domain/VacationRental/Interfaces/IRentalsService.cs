using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Domain.VacationRental.Interfaces
{
    public interface IRentalsService
    {
        Task<RentalViewModel> Get(int rentalId);
        Task<List<RentalViewModel>> Get();
        Task<ResourceIdViewModel> Post(RentalBindingModel model);
        Task<ResourceIdViewModel> Put(int rentalId, RentalBindingModel model);
    }
}
