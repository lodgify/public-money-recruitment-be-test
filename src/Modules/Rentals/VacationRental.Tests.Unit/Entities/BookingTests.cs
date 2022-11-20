using AutoFixture;
using Shouldly;
using VacationRental.Core.Entities;

namespace VacationRental.Tests.Unit.Entities
{
    public class BookingTests
    {
        private readonly Fixture _fixture;

        public BookingTests()
        {
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(1, 0, BookingStatus.Booked)]
        [InlineData(1, 1, BookingStatus.Preparation)]
        [InlineData(1, 2, BookingStatus.Free)]
        [InlineData(2, 0, BookingStatus.Booked)]
        [InlineData(2, 1, BookingStatus.Booked)]
        [InlineData(2, 2, BookingStatus.Preparation)]
        [InlineData(2, 3, BookingStatus.Free)]
        public void given_valid_booking_isBooked_should_return_valid_status_for_specified_date(int nights, int addDays, BookingStatus expectedStatus)
        {
            const int preparationTimeInDays = 1;
            var now = DateTime.UtcNow;
            var booking = CreateBooking(now, nights, preparationTimeInDays);

            var status = booking.GetStatus(now.AddDays(addDays));

            status.ShouldBe(expectedStatus);
        }

        private Booking CreateBooking(DateTime date, int nights, int preparationTimeInDays) => new(CreateRental(preparationTimeInDays), date, nights, _fixture.Create<int>());

        private Rental CreateRental(int preparationTimeInDays) => new(_fixture.Create<int>(), preparationTimeInDays);
    }
}
