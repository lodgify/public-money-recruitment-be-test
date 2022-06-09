using Microsoft.EntityFrameworkCore;
using VacationRental.DataAccess.Contexts;
using VacationRental.DataAccess.Models.Entities;

namespace VacationRental.Services.UnitTests
{
    public class VacationRentalSeedDataFixture : IDisposable
    {
        public VacationRentalDbContext VacationRentalDbContext { get; private set; }

        public VacationRentalSeedDataFixture()
        {
            var options = new DbContextOptionsBuilder<VacationRentalDbContext>().UseInMemoryDatabase("VacationRentalDbContext").Options;

            VacationRentalDbContext = new VacationRentalDbContext(options);

            VacationRentalDbContext?.Rentals?.Add(new Rental
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 1,
                IsActive = true,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            });

            VacationRentalDbContext?.Rentals?.Add(new Rental
            {
                Id = 2,
                Units = 4,
                PreparationTimeInDays = 1,
                IsActive = true,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            });

            VacationRentalDbContext?.Bookings?.AddRange(new[] {
                 new Booking {
                    Id = 1,
                    RentalId = 1,
                    Nights = 2,
                    Start = new DateTime(2000, 01, 02),
                    IsActive = true,
                    Created = DateTime.UtcNow
                 },
                 new Booking {
                    Id = 2,
                    RentalId = 1,
                    Nights = 2,
                    Start = new DateTime(2000, 01, 03),
                    IsActive = true,
                    Created = DateTime.UtcNow
                 },
                 new Booking {
                    Id = 3,
                    RentalId = 2,
                    Nights = 2,
                    Start = new DateTime(2000, 01, 01),
                    IsActive = true,
                    Created = DateTime.UtcNow
                 }
            });

            VacationRentalDbContext?.SaveChanges();
        }

        public void Dispose()
        {
            VacationRentalDbContext.Dispose();
        }
    }
}
