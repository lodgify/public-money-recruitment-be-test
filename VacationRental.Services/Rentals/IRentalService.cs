using VacationRental.Core.Domain.Rentals;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Services.Rentals
{
    public interface IRentalService
    {
        RentalViewModel Get(int rentalId);

        RentalEntity Add(RentalBindingModel request);
    }
}
