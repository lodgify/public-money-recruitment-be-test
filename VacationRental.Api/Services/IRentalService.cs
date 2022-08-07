using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalService
    {
        RentalViewModel GetRental(int rentalId);
        ResourceIdViewModel AddRental(RentalBindingModel model);
    }
}
