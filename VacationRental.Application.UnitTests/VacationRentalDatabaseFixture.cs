using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.UnitTests
{
    public class VacationRentalDatabaseFixture : IDisposable
    {
        public VacationRentalDbContext DbContext { get; private set; }

        public VacationRentalDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<VacationRentalDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            DbContext = new VacationRentalDbContext(options);
        }

        public async Task<Rental> CreateRental(int units, int preparationTimeInDays)
        {
            var rental = new Rental
            {
                Units = Enumerable.Range(0, units).Select(x => new Unit()).ToList(),
                PreparationTimeInDays = preparationTimeInDays
            };

            await DbContext.Rentals.AddAsync(rental);
            await DbContext.SaveChangesAsync();

            return rental;
        }

        public async Task<List<Booking>> GetRentalBookings(int rentalId)
        {
            var rental = await DbContext.Rentals.Include(x => x.Units).ThenInclude(x => x.Bookings).SingleAsync(x => x.Id == rentalId);
            return rental.Units.SelectMany(x => x.Bookings).ToList();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
