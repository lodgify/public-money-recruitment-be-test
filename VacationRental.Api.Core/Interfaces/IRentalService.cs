using LanguageExt;
using System.Threading.Tasks;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Core.Interfaces
{
    public interface IRentalService
    {
        Task<Result<ResourceIdViewModel>> InsertNewRentalAsync(RentalBindingModel rental);
        Task<Result<RentalViewModel>> GetRentalByIdAsync(int rentalId);
        Task<Result<ResourceIdViewModel>> UpdateRentalAsync(int rentalId, RentalBindingModel rental);
    }
}
