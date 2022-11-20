using Shouldly;
using VacationRental.Application.Commands;
using VacationRental.Application.Commands.Handlers;
using VacationRental.Core.Repositories;
using VacationRental.Infrastructure.EF.Repositories;
using VacationRental.Shared.Abstractions.Commands;
using VacationRental.Tests.Integration.Common;

namespace VacationRental.Tests.Integration.Commands
{
    public class AddRentalHandlerTests : IDisposable
    {
        private async Task<int> Act(AddRental command) => await _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_command_add_rental_should_succeed()
        {
            await _dbContext.Context.Database.EnsureCreatedAsync();

            const int units = 1;
            const int preparationTimeInDays = 2;

            var command = new AddRental(units, preparationTimeInDays);
            var newRentalId = await Act(command);

            var newRental = await _rentalRepository.GetAsync(newRentalId);
            newRental.Units.ShouldBe(units);
            newRental.PreparationTimeInDays.ShouldBe(preparationTimeInDays);
            newRental.Bookings.ShouldBeEmpty();
        }

        private readonly TestRentalsDbContext _dbContext;
        private readonly IRentalRepository _rentalRepository;
        private readonly ICommandHandler<AddRental, int> _handler;

        public AddRentalHandlerTests()
        {
            _dbContext = new TestRentalsDbContext();
            _rentalRepository = new RentalRepository(_dbContext.Context);
            _handler = new AddRentalHandler(_rentalRepository);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}