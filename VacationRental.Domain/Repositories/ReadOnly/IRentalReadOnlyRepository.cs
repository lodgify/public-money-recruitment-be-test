using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories.ReadOnly
{
    public interface IRentalReadOnlyRepository
    {
        Rental Get(RentalId id);
    }
}
