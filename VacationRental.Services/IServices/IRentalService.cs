using VacationRental.Domain.Models;

namespace VacationRental.Services.IServices
{
    public interface IRentalService
    {
        RentalViewModel Get(int id);
        ResourceIdViewModel Create(RentalBindingModel model);
        RentalViewModel Update(int id, RentalBindingModel rental);
    }
}
