using Microsoft.EntityFrameworkCore;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RentalSoftware.Infrastructure.Data.Repositories
{
    public class RentalRepository : RepositoryBase<Rental>, IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context) { }

        public async Task<Rental> GetRentalById(int rentalId)
        {
            return await Context.Rentals
                    .Where(x => x.Id == rentalId).AsNoTracking()
                    .FirstOrDefaultAsync();
        }
    }
}
