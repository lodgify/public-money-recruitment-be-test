using Microsoft.EntityFrameworkCore;
using System.Threading;
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

        public async Task AddAsync(Rental rental, CancellationToken cancellationToken)
        {
            await _rentals.AddAsync(rental, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Rental> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _rentals
                .Include(x => x.Bookings)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Rental rental, CancellationToken cancellationToken)
        {
            _rentals.Update(rental);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
