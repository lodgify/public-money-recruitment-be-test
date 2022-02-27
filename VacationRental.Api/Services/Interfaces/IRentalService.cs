using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IRentalService
    {
        ResourceIdViewModel Create(RentalBindingModel model);

        RentalViewModel Get(int rentalId);
    }
}
