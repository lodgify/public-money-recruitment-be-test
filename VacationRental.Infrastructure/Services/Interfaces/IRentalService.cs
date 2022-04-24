using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.DTOs;

namespace VacationRental.Infrastructure.Services.Interfaces
{
    public interface IRentalService
    {
        Rental GetRental(int id);

        int CreateRental(Rental rental);

        Rental UpdateRental(RentalUpdateInputDTO model, int id);
    }
}
