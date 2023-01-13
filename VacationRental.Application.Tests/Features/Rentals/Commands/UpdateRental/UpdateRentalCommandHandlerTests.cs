using AutoMapper;
using Moq;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Rentals.Commands.UpdateRental;
using VacationRental.Application.Mapping;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Tests.Features.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommandHandlerTests
    {
        private Mock<IRepository<Rental>> _rentalRepository;
        private Mock<IBookingRepository> _bookingRepository;
        private IMapper _mapper;

        public UpdateRentalCommandHandlerTests()
        {
            _rentalRepository = new();
            _bookingRepository = new();
            _mapper = TestData.CreateMapForTest();
        }

        [Fact]
        public void Handle_Should_UpdateAnExistingRental_WhenThereAreNotConfictsBetweenBookings()
        {                        
            //Arrange            
            var rental = TestData.CreateRentalForTest();
            var booking = TestData.CreateBookingForTest(rental.Id, DateTime.Now);

            var command = new UpdateRentalCommand(rental.Id, 2, 3);
            var handler = new UpdateRentalCommandHandler(_rentalRepository.Object, _bookingRepository.Object, _mapper);
            
            _bookingRepository.Setup(x => x.GetBookingByRentalId(It.IsAny<int>()))
                .Returns(new List<Booking> { booking });

            _rentalRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(rental);

            rental.SetUnits(command.Units);
            rental.SetPreparationTimeInDays(command.PreparationTimeInDays);

            _rentalRepository.Setup(x => x.Update(It.IsAny<Rental>()))
                .Returns(rental);

            //Act
            var result = handler.Handle(command);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(command.Units, result.Units);
            Assert.Equal(command.PreparationTimeInDays, result.PreparationTimeInDays);
        }

        [Fact]
        public void Handle_Should_ReturnConflictException_WhenThereAreAConflictWithRentalUnits()
        {
            //Arrange            
            var rental = TestData.CreateRentalForTest();
            var booking = TestData.CreateBookingForTest(rental.Id, DateTime.Now);

            var command = new UpdateRentalCommand(rental.Id, 0, 3);
            var handler = new UpdateRentalCommandHandler(_rentalRepository.Object, _bookingRepository.Object, _mapper);

            //Act
            _bookingRepository.Setup(x => x.GetBookingByRentalId(It.IsAny<int>()))
                .Returns(new List<Booking> { booking });                                   

            //Assert
            Assert.Throws<ConflictException>(() => handler.Handle(command));            
        }

        [Fact]
        public void Handle_Should_ReturnConflictException_WhenThereAreAConflictWithPreparationTimeInDaysOfRentals()
        {
            //Arrange            
            var rental = TestData.CreateRentalForTest();
            var booking = TestData.CreateBookingForTest(rental.Id, DateTime.Now);
            var booking2 = TestData.CreateBookingForTest(rental.Id, DateTime.Now.AddDays(2));

            var command = new UpdateRentalCommand(rental.Id, 1, 3);
            var handler = new UpdateRentalCommandHandler(_rentalRepository.Object, _bookingRepository.Object, _mapper);

            //Act
            _bookingRepository.Setup(x => x.GetBookingByRentalId(It.IsAny<int>()))
                .Returns(new List<Booking> { booking, booking2 });

            //Assert
            Assert.Throws<ConflictException>(() => handler.Handle(command));
        }

    }
}
