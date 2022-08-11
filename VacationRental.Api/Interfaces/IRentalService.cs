using System.Threading.Tasks;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Models;

namespace VacationRental.Api.Interfaces
{
    public interface IRentalService
    {
        RentalViewModel GetRental(int id);

        Task<ResourceIdViewModel> CreateAsync(RentalBindingModel model);

        Task UpdateAsync(UpdateRentalBindingModel model);
    }
}