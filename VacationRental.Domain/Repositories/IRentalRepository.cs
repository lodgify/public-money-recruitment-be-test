using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories.ReadOnly;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories
{
    public interface IRentalRepository : IRentalReadOnlyRepository
    {
        Rental Get(RentalId id);
        Rental Add(Rental rental);
    }
}
