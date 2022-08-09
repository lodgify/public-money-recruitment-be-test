using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public interface IRentalRepository
    {
        int RentalsCount();
        RentalViewModel Get(int id);
        
        int Create(RentalViewModel model);
    }
}