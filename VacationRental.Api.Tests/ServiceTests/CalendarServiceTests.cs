using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Api.Services.Interfaces;
using Xunit;

namespace VacationRental.Api.Tests.ServiceTests
{
    public class CalendarServiceTests
    {
        [Fact]
        public async Task Get_WhenRequestIsNull_ThenGetApplicationException()
        {
            // Arrange
            Mock<IBookingService> bookingService = new Mock<IBookingService>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            CalendarService sut = new CalendarService(bookingService.Object, rentalService.Object);

            CalendarBindingModel request = null;

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(request));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The supplied calendar model is not appropriate!");
        }

        [Fact]
        public async Task Get_WhenRentalIdIsNotGreaterThanZero_ThenGetApplicationException()
        {
            // Arrange
            Mock<IBookingService> bookingService = new Mock<IBookingService>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            CalendarService sut = new CalendarService(bookingService.Object, rentalService.Object);

            CalendarBindingModel request = new Bogus.Faker<CalendarBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(-10, 0));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(request));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Rental Id must be greater than zero!");
        }

        [Fact]
        public async Task Get_WhenStartIsNull_ThenGetApplicationException()
        {
            // Arrange
            Mock<IBookingService> bookingService = new Mock<IBookingService>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            CalendarService sut = new CalendarService(bookingService.Object, rentalService.Object);

            CalendarBindingModel request = new Bogus.Faker<CalendarBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(request));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Start date must be greater than minimum date!");
        }

        [Fact]
        public async Task Get_WhenNightsIsNull_ThenGetApplicationException()
        {
            // Arrange
            Mock<IBookingService> bookingService = new Mock<IBookingService>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            CalendarService sut = new CalendarService(bookingService.Object, rentalService.Object);

            CalendarBindingModel request = new Bogus.Faker<CalendarBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Start, DateTime.Now);

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(request));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Nights must be greater than zero!");
        }

        [Fact]
        public async Task Get_WhenRentalIdIsNotBelongingToAnyRental_ThenGetApplicationException()
        {
            // Arrange
            Mock<IBookingService> bookingService = new Mock<IBookingService>();
            Mock<IRentalService> rentalService = new Mock<IRentalService>();

            CalendarService sut = new CalendarService(bookingService.Object, rentalService.Object);

            CalendarBindingModel request = new Bogus.Faker<CalendarBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Start, DateTime.Now)
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10));

            // Act
            ApplicationException exception = Assert.Throws<ApplicationException>(() => sut.Get(request));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Rental not found with rentalId:{request.RentalId}!");
        }

        [Fact]
        public async Task Get_WhenRequestIsValid_ThenGetCalendarViewModel()
        {
            // Arrange
            int autoGenerationCount = new Bogus.Faker().Random.Number(1, 10);

            Mock<IBookingService> bookingService = CreateMockupBookingService(autoGenerationCount);
            Mock<IRentalService> rentalService = CreateMockupRentalService();

            CalendarService sut = new CalendarService(bookingService.Object, rentalService.Object);

            CalendarBindingModel request = new Bogus.Faker<CalendarBindingModel>()
                .RuleFor(r => r.RentalId, f => f.Random.Number(1, 10))
                .RuleFor(r => r.Start, DateTime.Now)
                .RuleFor(r => r.Nights, f => f.Random.Number(1, 10));

            // Act
            CalendarViewModel calendarViewModel = sut.Get(request);

            // Assert
            calendarViewModel.Should().NotBeNull();
            calendarViewModel.RentalId.Should().Be(request.RentalId);
            calendarViewModel.Dates.Count.Should().Be(request.Nights);
            calendarViewModel.Dates.First().Date.Should().Be(request.Start.Date);
            calendarViewModel.Dates.First().Bookings.Count.Should().Be(autoGenerationCount);
            calendarViewModel.Dates.First().Bookings.First().Unit.Should().BeInRange(1, 10);
            calendarViewModel.Dates.First().Preparations.Count.Should().Be(autoGenerationCount);
            calendarViewModel.Dates.First().Preparations.First().Unit.Should().BeInRange(1, 10);
        }

        private static Mock<IBookingService> CreateMockupBookingService(int count)
        {
            Mock<IBookingService> service = new Mock<IBookingService>();

            List<CalendarBookingViewModel> bookings = new Bogus.Faker<CalendarBookingViewModel>()
                .RuleFor(r => r.Id, f => f.IndexFaker + 1)
                .RuleFor(r => r.Unit, f => f.Random.Number(1, 10))
                .Generate(count);

            List<CalendarPreparationViewModel> preparations = new Bogus.Faker<CalendarPreparationViewModel>()
                .RuleFor(r => r.Unit, f => f.Random.Number(1, 10))
                .Generate(count);

            service.Setup(r => r.GetBookingsByDate(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(bookings);

            service.Setup(r => r.GetPreparationsByDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(preparations);

            return service;
        }

        private static Mock<IRentalService> CreateMockupRentalService()
        {
            Mock<IRentalService> service = new Mock<IRentalService>();

            service.Setup(r => r.Get(It.IsAny<int>()))
                .Returns(new RentalViewModel());

            return service;
        }
    }
}
