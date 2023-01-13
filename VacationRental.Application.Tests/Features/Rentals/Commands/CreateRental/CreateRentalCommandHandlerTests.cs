using Moq;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Features.Rentals.Commands.CreateRental;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Tests.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandHandlerTests
    {
        private Mock<IRepository<Rental>> _rentalRepository;

        public CreateRentalCommandHandlerTests()
        {
            _rentalRepository = new();
        }

        [Fact]
        public void Handle_Should_CreateNewRental_WhenRequestIsValid()
        {
            //Arrange
            var command = new CreateRentalCommand(1, 2);
            var handler = new CreateRentalCommandHandler(_rentalRepository.Object);
            var rental = TestData.CreateRentalForTest();

            //Act
            _rentalRepository.Setup(x => x.Add(It.IsAny<Rental>()))
                .Returns(rental);

            var result = handler.Handle(command);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<ResourceId>(result);
            Assert.Equal(rental.Id, result.Id);

        }
    }
}
