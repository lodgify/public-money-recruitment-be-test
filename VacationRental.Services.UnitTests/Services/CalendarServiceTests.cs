using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.DataAccess.Repositories;
using VacationRental.Infrastructure.Profiles;
using VacationRental.Services.Interfaces;
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
            _bookingRepository = new GenericRepository<Booking>(fixture.VacationRentalDbContext);
            _rentalRepository = new GenericRepository<Rental>(fixture.VacationRentalDbContext);

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
            const int nights = 5;
            var start = new DateTime(2000, 01, 01);
            
            // Action
            var result = await _service.GetCalendarAsync(rentalId, nights, start);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(5, result?.Dates?.Length);

            Assert.Equal(new DateTime(2000, 01, 01), result?.Dates[0]?.Date);
            Assert.Empty(result?.Dates[0]?.Bookings);
            Assert.Single(result?.Dates[0]?.PreparationTimes);

            Assert.Equal(new DateTime(2000, 01, 02), result?.Dates[1]?.Date);
            Assert.Single(result?.Dates[1]?.Bookings);
            
            Assert.Equal(new DateTime(2000, 01, 03), result.Dates[2].Date);
            Assert.Equal(2, result.Dates[2]?.Bookings?.Length);
            
            Assert.Equal(new DateTime(2000, 01, 04), result.Dates[3].Date);
            Assert.Single(result.Dates[3].Bookings);
            
            Assert.Equal(new DateTime(2000, 01, 05), result.Dates[4].Date);
        }
    }
}
