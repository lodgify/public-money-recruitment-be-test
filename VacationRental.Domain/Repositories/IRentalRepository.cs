using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories
{
    public interface IRentalRepository
    {
        Rental Get(RentalId id);
    }
}
