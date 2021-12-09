using RentalSoftware.Core.Entities;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Interfaces
{
    public interface IRentalRepository : IRepositoryBase<Rental>
    {
        Task<Rental> GetRentalById(int rentalId);
    }
}
