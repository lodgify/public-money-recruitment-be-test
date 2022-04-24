using VacationRental.Domain.Rentals;

namespace VacationRental.Infrastructure.Services.Interfaces
{
    public interface IRentalService
    {
        Rental GetRental(int id);

        int CreateRental(Rental rental);

        void UpdateRental(Rental rental, int id);
    }
}
