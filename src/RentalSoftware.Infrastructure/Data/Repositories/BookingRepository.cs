using Microsoft.EntityFrameworkCore;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RentalSoftware.Infrastructure.Data.Repositories
{
    public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context) { }

        public async Task<Booking> GetByIdAsync(int Id)
        {
            return await Context.Bookings
                    .Where(x => x.Id == Id)
                    .FirstOrDefaultAsync();
        }
    }
}
