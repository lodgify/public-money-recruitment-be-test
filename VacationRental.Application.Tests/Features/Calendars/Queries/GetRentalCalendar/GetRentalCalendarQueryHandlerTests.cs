using Moq;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Tests.Features.Calendars.Queries.GetRentalCalendar
{
    public class GetRentalCalendarQueryHandlerTests 
    {
        private Mock<IRepository<Rental>> _rentalRepository;
        private Mock<IBookingRepository> _bookingRepository;

        public GetRentalCalendarQueryHandlerTests()
        {
            _rentalRepository = new();
            _bookingRepository = new();
        }

        [Fact]
        public void Handler_Should_ReturnNotFoundException_WhenRentalDoesntExist()
        {
            //Arrange
            var request = new GetRentalCalendarQuery(1, DateTime.Now, 1);
            var handler = new GetRentalCalendarQueryHandler(_rentalRepository.Object, _bookingRepository.Object);
            Rental rental = null;
            //Act
            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(rental);

            //Assert
            Assert.Throws<NotFoundException>(() => handler.Handle(request));
        }

        [Fact]
        public void Handler_Should_ReturnCompleteCalendar_WhenRentalContainsSomeBookings()
        {
            //Arrange
            var request = new GetRentalCalendarQuery(1, DateTime.Now, 4);
            var handler = new GetRentalCalendarQueryHandler(_rentalRepository.Object, _bookingRepository.Object);
            var rental = TestData.CreateRentalForTest();
            var booking1 = TestData.CreateBookingForTest(rental.Id, DateTime.Now.AddDays(-1));
            var booking2 = TestData.CreateBookingForTest(rental.Id, DateTime.Now.AddDays(-2));
            var booking3 = TestData.CreateBookingForTest(rental.Id, DateTime.Now.AddDays(-3));
            //Act
            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(rental);

            _bookingRepository.Setup(x => x.GetBookingByRentalId(It.IsAny<int>()))
                .Returns(new List<Booking> { booking1, booking2, booking3 });
            
            var result = handler.Handle(request);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Dates);
            Assert.True(result.Dates.Count == 4);
            Assert.True(result.Dates[0].Bookings.Count == 2);
            Assert.True(result.Dates[0].PreparationTimes.Count == 1);
            Assert.True(result.Dates[1].Bookings.Count == 1);
            Assert.True(result.Dates[1].PreparationTimes.Count == 2);
            Assert.True(result.Dates[2].Bookings.Count == 0);
            Assert.True(result.Dates[2].PreparationTimes.Count == 2);
            Assert.True(result.Dates[3].Bookings.Count == 0);
            Assert.True(result.Dates[3].PreparationTimes.Count == 1);
        }
    }
}
