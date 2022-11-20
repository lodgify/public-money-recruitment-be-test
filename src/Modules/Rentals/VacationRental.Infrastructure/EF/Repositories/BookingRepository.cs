﻿using Microsoft.EntityFrameworkCore;
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

        public async Task AddAsync(Booking booking)
        {
            var latest = await _bookings.LastOrDefaultAsync();
            var newBookingId = latest is null ? 1 : latest.Id + 1;

            booking.SetBookingId(newBookingId);

            await _bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking> GetAsync(int id)
        {
            return await _bookings
                .Include(x => x.Rental)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
