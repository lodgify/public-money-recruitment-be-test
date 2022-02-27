using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Data;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Api.Services.Interfaces;
using Xunit;

namespace VacationRental.Api.Tests.ServiceTests
{
    public class BookingServiceTests
    {
        [Fact]
        public async Task Create_WhenRequestIsNull_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            BookingService sut = new BookingService(bookings, rentalService.Object);

            BookingBindingModel model = null;

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Create(model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The supplied booking is not appropriate!");
        }

        [Fact]
        public async Task Create_WhenRentalIdIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            BookingService sut = new BookingService(bookings, rentalService.Object);

            BookingBindingModel model = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(-10, 0));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Create(model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Rental Id must be greater than zero!");
        }

        [Fact]
        public async Task Create_WhenNightsIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            BookingService sut = new BookingService(bookings, rentalService.Object);

            BookingBindingModel model = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(-10, 0));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Create(model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Nights must be greater than zero!");
        }

        [Fact]
        public async Task Create_WhenStartIsNull_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            BookingService sut = new BookingService(bookings, rentalService.Object);

            BookingBindingModel model = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Create(model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Start date must be greater than minimum date!");
        }

        [Fact]
        public async Task Create_WhenRequestIsValid_ThenGetCreatedResourceIdViewModel()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = CreateMockupRentalService();

            BookingService sut = new BookingService(bookings, rentalService.Object);

            BookingBindingModel model = new Bogus.Faker<BookingBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Start, DateTime.Today);

            // Act
            ResourceIdViewModel resourceIdViewModel = sut.Create(model);

            // Assert
            resourceIdViewModel.Should().NotBeNull();
            resourceIdViewModel.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Get_WhenBookingIdIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            BookingService sut = new BookingService(bookings, rentalService.Object);

            int bookingId = new Bogus.Faker().Random.Number(-10, 0);

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(bookingId));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Booking Id must be greater than zero!");
        }

        [Fact]
        public async Task Get_WhenBookingIdIsNotBelongingToAnyBooking_ThenGetApplicationException()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            BookingService sut = new BookingService(bookings, rentalService.Object);

            int bookingId = new Bogus.Faker().Random.Number(1, 10);

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(bookingId));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Booking not found with bookingId: {bookingId}!");
        }

        [Fact]
        public async Task Get_WhenRentalIdIsValid_ThenGetCorrectBookingViewModel()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            Booking booking = new Bogus.Faker<Booking>()
                .RuleFor(r => r.Id, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Start, DateTime.Today);

            bookings.Add(1, booking);

            BookingService sut = new BookingService(bookings, rentalService.Object);

            // Act
            BookingViewModel bookingViewModel = sut.Get(booking.Id);

            // Assert
            bookingViewModel.Should().NotBeNull();
            bookingViewModel.Id.Should().Be(booking.Id);
            bookingViewModel.Nights.Should().Be(booking.Nights);
            bookingViewModel.RentalId.Should().Be(booking.RentalId);
            bookingViewModel.Start.Should().Be(booking.Start);
        }

        [Fact]
        public async Task GetBookingsByDate_WhenThereIsNoBookingForSelectedDate_ThenGetEmptyList()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = CreateMockupRentalService();

            int rentalId = rentalService.Object.Get(1).Id;

            Booking booking = new Bogus.Faker<Booking>()
                .RuleFor(r => r.Id, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, rentalId)
                .RuleFor(r => r.Start, DateTime.Today.AddMonths(1))
                .RuleFor(r => r.Unit, f => f.Random.Number(1, 10));

            bookings.Add(1, booking);

            BookingService sut = new BookingService(bookings, rentalService.Object);

            // Act
            IEnumerable<CalendarBookingViewModel> calendarBookingViewModels = sut.GetBookingsByDate(rentalId, DateTime.Now);

            // Assert
            calendarBookingViewModels.Should().NotBeNull();
            calendarBookingViewModels.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetBookingsByDate_WhenBookingsExist_ThenGetCorrectBookingList()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = CreateMockupRentalService();

            DateTime bookingDate = DateTime.Today;
            int rentalId = rentalService.Object.Get(1).Id;

            Booking booking = new Bogus.Faker<Booking>()
                .RuleFor(r => r.Id, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, rentalId)
                .RuleFor(r => r.Start, bookingDate)
                .RuleFor(r => r.Unit, f => f.Random.Number(1, 10));

            bookings.Add(1, booking);

            BookingService sut = new BookingService(bookings, rentalService.Object);

            // Act
            IEnumerable<CalendarBookingViewModel> calendarBookingViewModels = sut.GetBookingsByDate(rentalId, bookingDate);

            // Assert
            calendarBookingViewModels.Should().NotBeNull();
            calendarBookingViewModels.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetPreparationsByDate_WhenThereIsNoBookingForSelectedDate_ThenGetEmptyList()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = CreateMockupRentalService();

            int rentalId = rentalService.Object.Get(1).Id;
            int preparationTime = rentalService.Object.Get(1).PreparationTimeInDays;

            Booking booking = new Bogus.Faker<Booking>()
                .RuleFor(r => r.Id, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, rentalId)
                .RuleFor(r => r.Start, DateTime.Today.AddMonths(1))
                .RuleFor(r => r.Unit, f => f.Random.Number(1, 10));

            bookings.Add(1, booking);

            BookingService sut = new BookingService(bookings, rentalService.Object);

            // Act
            IEnumerable<CalendarPreparationViewModel> calendarBookingViewModels = sut.GetPreparationsByDate(rentalId, preparationTime, DateTime.Now);

            // Assert
            calendarBookingViewModels.Should().NotBeNull();
            calendarBookingViewModels.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetPreparationsByDate_WhenBookingsExist_ThenGetCorrectBookingList()
        {
            // Arrange
            Dictionary<int, Booking> bookings = new Dictionary<int, Booking>();
            Mock<IRentalService> rentalService = CreateMockupRentalService();

            DateTime bookingDate = DateTime.Today;
            int rentalId = rentalService.Object.Get(1).Id;
            int preparationTime = rentalService.Object.Get(1).PreparationTimeInDays;

            Booking booking = new Bogus.Faker<Booking>()
                .RuleFor(r => r.Id, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10))
                .RuleFor(r => r.RentalId, rentalId)
                .RuleFor(r => r.Start, bookingDate)
                .RuleFor(r => r.Unit, f => f.Random.Number(1, 10));

            bookings.Add(1, booking);

            BookingService sut = new BookingService(bookings, rentalService.Object);

            // Act
            IEnumerable<CalendarPreparationViewModel> calendarBookingViewModels = sut.GetPreparationsByDate(rentalId, preparationTime, bookingDate.AddDays(booking.Nights));

            // Assert
            calendarBookingViewModels.Should().NotBeNull();
            calendarBookingViewModels.Should().HaveCount(1);
        }

        private static Mock<IRentalService> CreateMockupRentalService()
        {
            Mock<IRentalService> service = new Mock<IRentalService>();

            RentalViewModel model = new Bogus.Faker<RentalViewModel>()
                .RuleFor(r => r.Id, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Units, f => f.Random.Number(1, 10))
                .RuleFor(r => r.PreparationTimeInDays, f => f.Random.Number(1, 10));

            service.Setup(r => r.Get(It.IsAny<int>()))
                .Returns(model);

            return service;
        }
    }
}
