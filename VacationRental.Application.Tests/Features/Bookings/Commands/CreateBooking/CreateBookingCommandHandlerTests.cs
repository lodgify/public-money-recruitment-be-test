using Moq;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Bookings.Commands.CreateBooking;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Tests.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandlerTests
    {

        private Mock<IBookingRepository> _bookingRepository;
        private Mock<IRepository<Rental>> _rentalRepository;

        public CreateBookingCommandHandlerTests()
        {
            _bookingRepository = new();
            _rentalRepository = new();
        }

        [Fact]
        public void Handle_Should_ThrownNotFoundException_WhenRentalDoesntExists()
        {
            //Arrange
            var request = new CreateBookingCommand(1, DateTime.Now, 1, 1);
            var commandHandler = new CreateBookingCommandHandler(_bookingRepository.Object, _rentalRepository.Object);
            Rental rental = null;

            //Act
            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(rental);
            
            //Assert
            Assert.Throws<NotFoundException>(() => commandHandler.Handle(request));
        }

        [Fact]
        public void Handle_Should_ThrownConflictException_WhenRentalIsNotAvailable()
        {
            //Arrange
            var request = new CreateBookingCommand(1, DateTime.Now, 1, 1);
            var commandHandler = new CreateBookingCommandHandler(_bookingRepository.Object, _rentalRepository.Object);
            Rental rental = TestData.CreateRentalForTest();
            Booking booking = TestData.CreateBookingForTest(rental.Id, DateTime.Now);
            Booking booking2 = TestData.CreateBookingForTest(rental.Id, DateTime.Now.AddDays(1));

            //Act
            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(rental);

            _bookingRepository.Setup(x => x.GetBookingByRentalId(It.IsAny<int>()))
                .Returns(new List<Booking> { booking, booking2 });

            //Assert
            Assert.Throws<ConflictException>(() => commandHandler.Handle(request));
        }

        [Fact]
        public void Handle_Should_CreateABooking_WhenRentalIsAvailable()
        {
            //Arrange
            var request = new CreateBookingCommand(1, DateTime.Now, 1, 1);
            var commandHandler = new CreateBookingCommandHandler(_bookingRepository.Object, _rentalRepository.Object);
            Rental rental = TestData.CreateRentalForTest();
            Booking booking = TestData.CreateBookingForTest(rental.Id, DateTime.Now);            

            //Act
            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(rental);

            _bookingRepository.Setup(x => x.GetBookingByRentalId(It.IsAny<int>()))
                .Returns(new List<Booking> { });

            _bookingRepository.Setup(x => x.Add(It.IsAny<Booking>()))
                .Returns(booking);

            var result = commandHandler.Handle(request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, booking.Id);
        }

    }
}
