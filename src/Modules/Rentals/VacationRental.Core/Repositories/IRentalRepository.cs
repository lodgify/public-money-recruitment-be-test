using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Repositories
{
    internal interface IRentalRepository
    {
        Task<Rental> GetAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(Rental rental, CancellationToken cancellationToken);
        Task UpdateAsync(Rental rental, CancellationToken cancellationToken);
    }
}
