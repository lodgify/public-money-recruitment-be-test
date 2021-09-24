using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories.ReadOnly;

namespace VacationRental.Domain.Repositories
{
    public interface IRentalRepository : IRentalReadOnlyRepository
    {
        ValueTask<Rental> Add(Rental rental);
    }
}
