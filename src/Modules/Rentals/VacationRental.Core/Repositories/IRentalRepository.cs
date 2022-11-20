using System.Threading.Tasks;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Repositories
{
    internal interface IRentalRepository
    {
        Task<Rental> GetAsync(int id);
        Task AddAsync(Rental rental);
    }
}
