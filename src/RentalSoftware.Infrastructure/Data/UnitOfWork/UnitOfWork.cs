using RentalSoftware.Core.Interfaces;
using RentalSoftware.Infrastructure.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace RentalSoftware.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRentalRepository RentalRepository { get; }

        public IBookingRepository BookingRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            RentalRepository = new RentalRepository(_context);
            BookingRepository = new BookingRepository(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
