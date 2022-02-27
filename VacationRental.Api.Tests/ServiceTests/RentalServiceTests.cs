using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Data;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using Xunit;

namespace VacationRental.Api.Tests.ServiceTests
{
    public class RentalServiceTests
    {
        [Fact]
        public async Task Create_WhenRequestIsNull_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            RentalBindingModel model = null;

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Create(model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The supplied rental model is not appropriate!");
        }

        [Fact]
        public async Task Create_WhenUnitsIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            RentalBindingModel model = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(-10, 0));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Create(model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Units must be greater than zero!");
        }

        [Fact]
        public async Task Create_WhenPreparationTimeInDaysIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            RentalBindingModel model = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(1, 10))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(-10, 0));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Create(model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("PreparationTimeInDays must be greater than zero!");
        }

        [Fact]
        public async Task Create_WhenRequestIsValid_ThenGetCreatedRentalViewModel()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            RentalBindingModel model = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(1, 10))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(1, 10));

            // Act
            ResourceIdViewModel resourceIdViewModel = sut.Create(model);

            // Assert
            resourceIdViewModel.Should().NotBeNull();
            resourceIdViewModel.Id.Should().Be(1);
        }

        [Fact]
        public async Task Get_WhenRentalIdIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            int rentalId = new Bogus.Faker().Random.Number(-10, 0);

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(rentalId));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Rental Id must be greater than zero!");
        }

        [Fact]
        public async Task Get_WhenRentalIdIsNotBelongingToAnyRental_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            int rentalId = new Bogus.Faker().Random.Number(1, 10);

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(rentalId));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Rental not found with rentalId:{rentalId}!");
        }

        [Fact]
        public async Task Get_WhenRentalIdIsValid_ThenGetCorrectRental()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            int rentalKey = new Bogus.Faker().Random.Number(1, 10);

            Rental rental = new Bogus.Faker<Rental>()
                .RuleFor(r => r.Id, f => f.Random.Number(1, 10))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Units, f => f.Random.Number(1, 10));

            rentals.Add(rentalKey, rental);

            RentalService sut = new RentalService(bookings, rentals);

            // Act
            RentalViewModel rentalViewModel = sut.Get(rental.Id);

            // Assert
            rentalViewModel.Should().NotBeNull();
            rentalViewModel.Id.Should().Be(rental.Id);
            rentalViewModel.PreparationTimeInDays.Should().Be(rental.PreparationTimeInDays);
            rentalViewModel.Units.Should().Be(rental.Units);
        }

        [Fact]
        public async Task Update_WhenRentalIdIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            int rentalId = new Bogus.Faker().Random.Number(-10, 0);

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Update(rentalId, null));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Rental Id must be greater than zero!");
        }

        [Fact]
        public async Task Update_WhenRentalBindingModelIsNull_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            int rentalId = new Bogus.Faker().Random.Number(1, 10);

            RentalBindingModel model = null;

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Update(rentalId, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The supplied rental model is not appropriate!");
        }

        [Fact]
        public async Task Update_WhenRentalIdIsNotBelongingToAnyRental_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            RentalService sut = new RentalService(bookings, rentals);

            int rentalId = new Bogus.Faker().Random.Number(1, 10);

            RentalBindingModel model = new RentalBindingModel();

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Update(rentalId, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Rental not found with rentalId:{rentalId}!");
        }

        [Fact]
        public async Task Update_WhenUnitNumberIsNotSufficient_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Rental> rentals = CreateMockupRentals();
            Dictionary<int, Booking> bookings = CreateMockupBookings(rentals.First().Value.Id);

            RentalService sut = new RentalService(bookings, rentals);

            RentalBindingModel model = new RentalBindingModel()
            {
                Units = 1
            };

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Update(rentals.First().Value.Id, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Unit number is not sufficient!");
        }

        [Fact]
        public async Task Update_WhenNewPreparationTimeCauseOverlap_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Rental> rentals = CreateMockupRentals();
            Dictionary<int, Booking> bookings = CreateMockupBookings(rentals.First().Value.Id);

            Booking booking = bookings.Values.First();

            Booking newBookingToOverlap = new Booking()
            {
                Id = 81,
                RentalId = booking.RentalId,
                Nights = booking.Nights,
                Start = booking.Start.AddDays(booking.Nights + 1),
                Unit = booking.Unit
            };

            bookings.Add(3, newBookingToOverlap);

            RentalService sut = new RentalService(bookings, rentals);

            RentalBindingModel model = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(11, 20))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(11, 20));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Update(rentals.First().Value.Id, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The process can not be done! An overlapping occurred due to reducing the number of units!");
        }

        [Fact]
        public async Task Update_WhenRequestIsValid_ThenGetUpdatedRental()
        {
            // Arrange
            Dictionary<int, Rental> rentals = CreateMockupRentals();
            Dictionary<int, Booking> bookings = CreateMockupBookings(rentals.First().Value.Id);

            RentalService sut = new RentalService(bookings, rentals);

            RentalBindingModel model = new Bogus.Faker<RentalBindingModel>()
                .RuleFor(r => r.Units, f => f.Random.Number(11, 20))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(1, 4));

            // Act
            RentalViewModel rentalViewModel = sut.Update(rentals.First().Value.Id, model);

            // Assert
            rentalViewModel.Should().NotBeNull();
            rentalViewModel.Units.Should().Be(model.Units);
            rentalViewModel.PreparationTimeInDays.Should().Be(model.PreparationTimeInDays);
        }

        private static Dictionary<int, Booking> CreateMockupBookings(int rentalId)
        {
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();

            int bookingId = new Bogus.Faker().Random.Number(1, 10);

            for (int i = 0; i < 2; i++)
            {
                Booking booking = new Bogus.Faker<Booking>()
                    .RuleFor(x => x.Id, f => f.Random.Number(1, 1000))
                    .RuleFor(x => x.Nights, f => f.Random.Number(1, 10))
                    .RuleFor(x => x.RentalId, rentalId)
                    .RuleFor(x => x.Unit, f => f.Random.Number(1, 10))
                    .RuleFor(x => x.Start, DateTime.Now);

                bookings.Add(i, booking);
            }

            return bookings;
        }

        private Dictionary<int, Rental> CreateMockupRentals()
        {
            Dictionary<int, Rental> rentals = new Dictionary<int, Rental>();

            int rentalId = new Bogus.Faker().Random.Number(1, 10);

            Rental rental = new Bogus.Faker<Rental>()
                .RuleFor(x => x.Id, f => f.Random.Number(1, 10))
                .RuleFor(x => x.PreparationTimeInDays, f => f.Random.Number(5, 10))
                .RuleFor(x => x.Units, f => f.Random.Number(6, 10));

            rentals.Add(rentalId, rental);

            return rentals;
        }
    }
}
