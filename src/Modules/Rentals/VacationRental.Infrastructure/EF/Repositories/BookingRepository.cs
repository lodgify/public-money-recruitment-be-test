using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Repositories;

namespace VacationRental.Infrastructure.EF.Repositories
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly RentalsDbContext _context;
        private readonly DbSet<Booking> _bookings;

        public BookingRepository(RentalsDbContext context)
        {
            _context = context;
            _bookings = _context.Bookings;
        }

        public async Task<Booking> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _bookings
                .Include(x => x.Rental)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
