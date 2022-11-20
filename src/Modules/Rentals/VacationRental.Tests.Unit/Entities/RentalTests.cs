using AutoFixture;
using Shouldly;
using VacationRental.Core.Entities;
using VacationRental.Core.Exceptions;

namespace VacationRental.Tests.Unit.Entities
{
    public class RentalTests
    {
        private readonly Fixture _fixture;

        public RentalTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void given_valid_rental_update_should_succeed_when_it_is_possible()
        {
            var now = DateTime.UtcNow;
            const int units = 1;
            const int preparationTimeInDays = 1;
            const int nights = 1;
            var rental = CreateRental(units, preparationTimeInDays);
            rental.AddBooking(rental.CreateBooking(now, nights));
            rental.AddBooking(rental.CreateBooking(now.AddDays(preparationTimeInDays + 2), nights));

            Should.NotThrow(() => rental.Update(units, preparationTimeInDays + 1));
        }

        [Fact]
        public void given_valid_rental_update_should_fails_when_it_is_not_possible_due_to_units_change()
        {
            var now = DateTime.UtcNow;
            const int units = 2;
            const int preparationTimeInDays = 1;
            const int nights = 1;
            var rental = CreateRental(units, preparationTimeInDays);
            rental.AddBooking(rental.CreateBooking(now, nights));
            rental.AddBooking(rental.CreateBooking(now, nights));

            Should.Throw<RentalUpdateNotPossibleException>(() => rental.Update(units - 1, preparationTimeInDays));
        }

        [Fact]
        public void given_valid_rental_update_should_fails_when_it_is_not_possible_due_to_preparation_change()
        {
            var now = DateTime.UtcNow;
            const int units = 1;
            const int preparationTimeInDays = 1;
            const int nights = 1;
            var rental = CreateRental(units, preparationTimeInDays);
            rental.AddBooking(rental.CreateBooking(now, nights));
            rental.AddBooking(rental.CreateBooking(now.AddDays(preparationTimeInDays + 1), nights));

            Should.Throw<RentalUpdateNotPossibleException>(() => rental.Update(units, preparationTimeInDays + 1));
        }

        private Rental CreateRental(int units, int preparationTimeInDays) => new(units, preparationTimeInDays);
    }
}
