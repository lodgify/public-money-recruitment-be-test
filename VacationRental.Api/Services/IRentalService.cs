using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalService
    {
        RentalViewModel Get(int rentalId);
        ResourceIdViewModel Create(RentalBindingModel model);
        void Update(int id, RentalBindingModel model);
    }
}
