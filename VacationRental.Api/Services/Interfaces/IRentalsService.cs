using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IRentalsService : IGeneralService<RentalViewModel>
    {
        ResourceIdViewModel AddRental(RentalBindingModel rental);
    }
}
