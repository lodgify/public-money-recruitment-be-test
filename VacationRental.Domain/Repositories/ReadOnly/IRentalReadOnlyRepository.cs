using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Values;

namespace VacationRental.Domain.Repositories.ReadOnly
{
    public interface IRentalReadOnlyRepository
    {
        ValueTask<Rental> Get(RentalId id);
    }
}
