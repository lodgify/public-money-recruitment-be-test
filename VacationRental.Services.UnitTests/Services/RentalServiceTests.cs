using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.DataAccess.Repositories;
using VacationRental.Infrastructure.Profiles;
using VacationRental.Models.Exceptions;
using Xunit;

namespace VacationRental.Services.UnitTests.Services
{
    public class RentalServiceTests : IClassFixture<VacationRentalSeedDataFixture>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Rental> _rentalRepository;

        private readonly RentalService _rentalService;

        public RentalServiceTests(VacationRentalSeedDataFixture fixture)
        {
            if (!fixture.VacationRentalDbContext.Rentals.Any())
            {
                fixture.VacationRentalDbContext?.Rentals?.Add(new Rental
                {
                    Id = 4,
                    Units = 1,
                    PreparationTimeInDays = 1,
                    IsActive = true,
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow
                });
            }

            if (!fixture.VacationRentalDbContext.Bookings.Any())
            {
                fixture.VacationRentalDbContext?.Bookings?.AddRange(new[] {
                     new Booking {
                        Id = 5,
                        RentalId = 4,
                        Nights = 2,
                        Start = new DateTime(2001, 02, 10),
                        IsActive = true,
                        Created = DateTime.UtcNow
                     }
                });

                fixture.VacationRentalDbContext?.Bookings?.AddRange(new[] {
                     new Booking {
                        Id = 6,
                        RentalId = 4,
                        Nights = 2,
                        Start = new DateTime(2001, 03, 10),
                        IsActive = true,
                        Created = DateTime.UtcNow
                     }
                });
            }

            fixture.VacationRentalDbContext?.SaveChanges();

            _bookingRepository = new GenericRepository<Booking>(fixture.VacationRentalDbContext);
            _rentalRepository = new GenericRepository<Rental>(fixture.VacationRentalDbContext);

            var configuration = new MapperConfiguration(configure => {
                configure.AddProfile<BookingProfile>();
                configure.AddProfile<RentalProfile>();
            });
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<RentalService>>();

            _rentalService = new RentalService(_bookingRepository, _rentalRepository, mapper, loggerMock.Object);
        }

        [Fact]
        public async Task Rental_ShouldUpdateRentalToDecreaseUnitsWithBookings_Fail()
        {
            // Arrange

            // Action & Assert
            await Assert.ThrowsAsync<RentalInvalidException>(async() => await _rentalService.UpdateRentalAsync(1, new Models.Paramaters.RentalParameters { Units = 1, PreparationTimeInDays = 0 }));
        }
    }
}
