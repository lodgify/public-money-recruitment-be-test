using Moq;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Handlers;
using VacationRental.Core.Entities;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Tests.Unit.Commands
{
    public class AddRentalHandlerTests
    {
        private async Task Act(AddRental command) => await _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_command_add_rental_should_succeed()
        {
            const int units = 1;
            const int preparationTimeInDays = 2;

            var command = new AddRental(units, preparationTimeInDays);
            await Act(command);

            _rentalRepository.Verify(x => x.AddAsync(It.Is<Rental>(r => r.Units == units && r.PreparationTimeInDays == preparationTimeInDays)), Times.Once);
        }

        private readonly Mock<IRentalRepository> _rentalRepository;
        private readonly ICommandHandler<AddRental, int> _handler;

        public AddRentalHandlerTests()
        {
            _rentalRepository = new Mock<IRentalRepository>();
            _handler = new AddRentalHandler(_rentalRepository.Object);
        }
    }
}
