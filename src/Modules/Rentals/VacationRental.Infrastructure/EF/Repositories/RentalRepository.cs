using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Repositories;

namespace VacationRental.Infrastructure.EF.Repositories
{
    internal class RentalRepository : IRentalRepository
    {
        private readonly RentalsDbContext _context;
        private readonly DbSet<Rental> _rentals;

        public RentalRepository(RentalsDbContext context)
        {
            _context = context;
            _rentals = _context.Rentals;
        }

        public async Task AddAsync(Rental rental)
        {
            var latest = await _rentals.LastOrDefaultAsync();
            var newRentalId = latest is null ? 1 : latest.Id + 1;

            rental.SetRentalId(newRentalId);

            await _rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task<Rental> GetAsync(int id)
        {
            return await _rentals
                .Include(x => x.Bookings)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
