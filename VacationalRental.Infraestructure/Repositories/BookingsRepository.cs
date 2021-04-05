using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Infrastructure.DbContexts;

namespace VacationalRental.Infrastructure.Repositories
{
    public class BookingsRepository : IBookingsRepository
    {
        private readonly VacationRentalDbContext _vacationalRentalDbContext;

        public BookingsRepository(VacationRentalDbContext vacationalRentalDbContext)
        {
            _vacationalRentalDbContext = vacationalRentalDbContext;
        }

        public async Task<IEnumerable<BookingEntity>> GetBookings()
        {
            return await _vacationalRentalDbContext.BookingEntities.ToListAsync();
        }

        public async Task<BookingEntity> GetBookingById(int bookingId)
        {
            return await _vacationalRentalDbContext.BookingEntities.FindAsync(bookingId);
        }

        public async Task<int> InsertBooking(BookingEntity bookingEntity)
        {
            await _vacationalRentalDbContext.AddAsync(bookingEntity);

            await _vacationalRentalDbContext.SaveChangesAsync();

            return bookingEntity.Id;
        }

        public async Task<bool> BookingExists(int bookingId)
        {
            return await _vacationalRentalDbContext.BookingEntities.AnyAsync(a => a.Id == bookingId);
        }
    }
}
