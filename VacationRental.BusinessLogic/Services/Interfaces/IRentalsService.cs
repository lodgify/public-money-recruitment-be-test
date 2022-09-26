using VacationRental.BusinessObjects;

namespace VacationRental.BusinessLogic.Services.Interfaces
{
    public interface IRentalsService
    {
        Rental GetRental(int rentalId);
        int CreateRental(CreateRental createRental);
    }
}
