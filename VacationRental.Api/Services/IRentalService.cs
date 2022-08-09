using System.Threading.Tasks;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalService
    {
        RentalViewModel GetRental(int id);

        ResourceIdViewModel Create(RentalBindingModel model);
    }
}