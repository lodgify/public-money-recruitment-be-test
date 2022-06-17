using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.DataAccess.Repositories;
using VacationRental.Infrastructure.Profiles;
using VacationRental.Models.Exceptions;
using VacationRental.Services.UnitTests.Common;
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
            if (!fixture.VacationRentalDbContext.Rentals.Any())
            {
                fixture.VacationRentalDbContext?.Rentals?.Add(new Rental
                {
                    Id = 2,
                    Units = 2,
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
                        Id = 3,
                        RentalId = 2,
                        Nights = 2,
                        Start = new DateTime(2001, 01, 02),
                        IsActive = true,
                        Created = DateTime.UtcNow
                     },
                     new Booking {
                        Id = 4,
                        RentalId = 2,
                        Nights = 2,
                        Start = new DateTime(2001, 01, 03),
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
            // Arrange
            var parameters = new Models.Paramaters.BookingParameters
            {
                Nights = 2,
                RentalId = 2,
                Start = new DateTime(2001, 01, 02)
            };

            // Action & Assert
            await Assert.ThrowsAsync<BookingInvalidException>(async () => await _bookingService.AddBookingAsync(parameters));
        }
    }
}
