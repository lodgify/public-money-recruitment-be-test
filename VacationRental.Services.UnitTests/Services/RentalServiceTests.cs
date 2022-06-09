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
