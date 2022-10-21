using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Core.Interfaces
{
    public interface IRentalRepository
    {
        ResourceIdViewModel InsertNewRental(RentalBindingModel rental);
        RentalViewModel GetRental(int rentalId);
    }
}
