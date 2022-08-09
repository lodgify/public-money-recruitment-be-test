using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public interface IRentalRepository
    {
        int RentalsCount();
        RentalViewModel GetRental(int id);
        
        int CreateRental(RentalViewModel model);
    }
}