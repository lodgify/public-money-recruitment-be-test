using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalService
    {
        RentalViewModel Get(int rentalId);
        int Create(RentalBindingModel model);
        void Update(int id, RentalBindingModel model);
    }
}
