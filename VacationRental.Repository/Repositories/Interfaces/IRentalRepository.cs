using VacationRental.Repository.Entities;

namespace VacationRental.Repository.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        RentalEntity GetRentalEntity(int rentalId);
        int CreateRentalEntity(RentalEntity rentalEntity);
    }
}
