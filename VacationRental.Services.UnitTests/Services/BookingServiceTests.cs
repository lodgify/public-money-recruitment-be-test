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
    public class BookingServiceTests : IClassFixture<VacationRentalSeedDataFixture>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Rental> _rentalRepository;

        private readonly BookingService _bookingService;

        public BookingServiceTests(VacationRentalSeedDataFixture fixture)
        {
            _bookingRepository = new GenericRepository<Booking>(fixture.VacationRentalDbContext);
            _rentalRepository = new GenericRepository<Rental>(fixture.VacationRentalDbContext);

            var configuration = new MapperConfiguration(configure => {
                configure.AddProfile<BookingProfile>();
                configure.AddProfile<RentalProfile>();
            });
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<BookingService>>();

            _bookingService = new BookingService(_bookingRepository, _rentalRepository, mapper, loggerMock.Object);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            // Arrange
            const int bookingId = 3;
            const int rentalId = 2;

            // Action
            var result = await _bookingService.GetBookingByIdAsync(bookingId);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(rentalId, result.RentalId);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            await Assert.ThrowsAsync<BookingInvalidException>(async () => await _bookingService.AddBookingAsync(new Models.Paramaters.BookingParameters { 
                Nights = 2,
                RentalId = 1,
                Start = new DateTime(2000, 01, 02)
            }));
        }
    }
}
