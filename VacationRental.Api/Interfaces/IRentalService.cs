using LanguageExt;
using VacationRental.Api.Core.Models;

namespace VacationRental.Api.Interfaces
{
    public interface IRentalService
    {
        Result<RentalViewModel> GetRentalById(int rentalId);
        Result<ResourceIdViewModel> AddNewRental(RentalBindingModel model);
    }
}
