using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.DataAccess.Repositories;
using VacationRental.Infrastructure.Profiles;
using VacationRental.Services.Interfaces;
using VacationRental.Services.UnitTests.Common;
using Xunit;

namespace VacationRental.Services.UnitTests.Services
{
    public class CalendarServiceTests : IClassFixture<VacationRentalSeedDataFixture>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Rental> _rentalRepository;

        private readonly ICalendarService _service;

        public CalendarServiceTests(VacationRentalSeedDataFixture fixture)
        {
            if (!fixture.VacationRentalDbContext.Rentals.Any())
            {
                fixture.VacationRentalDbContext?.Rentals?.Add(new Rental
                {
                    Id = 1,
                    Units = 2,
                    PreparationTimeInDays = 2,
                    IsActive = true,
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow
                });
            }

            if (!fixture.VacationRentalDbContext.Bookings.Any())
            {
                fixture.VacationRentalDbContext?.Bookings?.AddRange(new[] {
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
                     }
                });
            }

            fixture.VacationRentalDbContext?.SaveChanges();

            _bookingRepository = new GenericRepository<Booking>(fixture.VacationRentalDbContext!);
            _rentalRepository = new GenericRepository<Rental>(fixture.VacationRentalDbContext!);

            var configuration = new MapperConfiguration(configure => {
                configure.AddProfile<BookingProfile>();
                configure.AddProfile<RentalProfile>();
            });
            var mapper = new Mapper(configuration);

            var logger = new Mock<ILogger<CalendarService>>();

            _service = new CalendarService(_bookingRepository, _rentalRepository, mapper, logger.Object);
        }

        [Fact]
        public async Task Calendar_ShouldGetCalendar_Success()
        {
            // Arrange
            const int rentalId = 1;
            const int nights = 6;
            var start = new DateTime(2000, 01, 01);
            
            // Action
            var result = await _service.GetCalendarAsync(rentalId, nights, start);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(6, result.Dates.Length);

            Assert.Equal(new DateTime(2000, 01, 01), result.Dates[0].Date);
            Assert.Empty(result.Dates[0].Bookings);
            Assert.Empty(result?.Dates[0]?.PreparationTimes);

            Assert.Equal(new DateTime(2000, 01, 02), result?.Dates[1]?.Date);
            Assert.Single(result?.Dates[1]?.Bookings);
            
            Assert.Equal(new DateTime(2000, 01, 03), result.Dates[2].Date);
            Assert.Equal(2, result.Dates[2]?.Bookings?.Length);
            
            Assert.Equal(new DateTime(2000, 01, 04), result.Dates[3].Date);
            Assert.Single(result.Dates[3].Bookings);
            
            Assert.Equal(new DateTime(2000, 01, 05), result.Dates[4].Date);
            Assert.Single(result.Dates[4].PreparationTimes);

            Assert.Equal(new DateTime(2000, 01, 06), result.Dates[5].Date);
            Assert.Single(result.Dates[5].PreparationTimes);
        }
    }
}
