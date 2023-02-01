using VacationRental.Api.Models;

namespace VacationRental.Api.Services;

public interface IRentalService
{
    bool IsExists(int rentalId);
    RentalViewModel Get(int rentalId);
    ResourceIdViewModel Add(RentalViewModel viewModel);
}