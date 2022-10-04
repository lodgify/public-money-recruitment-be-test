using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Bookings.Commands.CreateBooking;
using Xunit;

namespace VacationRental.Application.UnitTests
{
    public class CreateBookingHandlerTests 
        : RequestHandlerTestBase<CreateBookingHandler, CreateBookingCommand, ResourceIdViewModel>,
          IClassFixture<VacationRentalDatabaseFixture>
    {
        private readonly VacationRentalDatabaseFixture _dbFixture;

        public CreateBookingHandlerTests(VacationRentalDatabaseFixture dbFixture)
            :base(dbFixture.DbContext)
        {
            _dbFixture = dbFixture;
        }

        [Fact]
        public async Task WhenDataIsCorrect_ShouldBeExecutedWithoutErrors()
        {
            // Arrange
            var rental = await _dbFixture.CreateRental(1, 1);

            var command = new CreateBookingCommand
            {
                RentalId = rental.Id,
                Nights = 3,
                Start = DateTime.Today
            };

            // Act
            Func<Task> action = async () => await Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().NotThrowAsync();

            var bookings = await _dbFixture.GetRentalBookings(rental.Id);
            bookings.Should().HaveCount(1);

            var booking = bookings.First();
            booking.Start.Should().Be(command.Start);
            booking.End.Should().Be(command.Start.AddDays(command.Nights));
        }

        [Fact]
        public async Task WhenRentalNotExists_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                RentalId = 1,
                Nights = 3,
                Start = DateTime.Today
            };

            // Act
            Func<Task> action = async () => await Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task WhenRentalDoNotHaveAvailableUnits_ShouldThrowValidationException()
        {
            // Arrange
            var rental = await _dbFixture.CreateRental(1, 1);

            var command = new CreateBookingCommand
            {
                RentalId = 1,
                Nights = 3,
                Start = DateTime.Today
            };
            await Handler.Handle(command, CancellationToken.None);

            // Act
            Func<Task> action = async () => await Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<ValidationException>();
        }
    }
}
