using VacationRental.Api.Host.IntegrationTests.Common;
using VacationRental.Models.Paramaters;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly VacationRentalApplication _vacationRentalApplication;

        public GetCalendarTests()
        {
            _vacationRentalApplication = new VacationRentalApplication();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            // Get guest token
            var guestTokenResult = await _vacationRentalApplication.GetGuestTokenAsync();

            // Add rental
            var rentalParameters = new RentalParameters
            {
                Units = 2,
                PreparationTimeInDays = 2
            };
            var rentalResult = await _vacationRentalApplication.AddRentalAsync(guestTokenResult.AccessToken!, rentalParameters);

            // Add Booking #1
            var firstBookingParameters = new BookingParameters
            {
                RentalId = rentalResult?.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 02)
            };
            var firstBookingResult = await _vacationRentalApplication.AddBookingAsync(guestTokenResult.AccessToken!, firstBookingParameters);

            // Add Booking #2
            var secondBookingParameters = new BookingParameters
            {
                RentalId = rentalResult?.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 03)
            };
            var secondBookingResult = await _vacationRentalApplication.AddBookingAsync(guestTokenResult.AccessToken!, secondBookingParameters);

            // Get calendar
            var calendarResult = await _vacationRentalApplication.GetCalendarAsync(guestTokenResult.AccessToken!, rentalResult.Id);

            Assert.Equal(rentalResult.Id, calendarResult.RentalId);
            Assert.Equal(5, calendarResult.Dates?.Length);

            Assert.Equal(new DateTime(2002, 01, 01), calendarResult.Dates[0].Date);
            Assert.Empty(calendarResult.Dates[0].Bookings);

            Assert.Equal(new DateTime(2002, 01, 02), calendarResult.Dates[1].Date);
            Assert.Single(calendarResult.Dates[1].Bookings);
            Assert.Contains(calendarResult.Dates[1].Bookings, x => x.Id == firstBookingResult.Id);

            Assert.Equal(new DateTime(2002, 01, 03), calendarResult.Dates[2].Date);
            Assert.Equal(2, calendarResult.Dates[2].Bookings?.Length);
            Assert.Contains(calendarResult.Dates[2].Bookings, x => x.Id == firstBookingResult.Id);
            Assert.Contains(calendarResult.Dates[2].Bookings, x => x.Id == secondBookingResult.Id);

            Assert.Equal(new DateTime(2002, 01, 04), calendarResult.Dates[3].Date);
            Assert.Single(calendarResult.Dates[3].Bookings);
            Assert.Contains(calendarResult.Dates[3].Bookings, x => x.Id == secondBookingResult.Id);

            Assert.Equal(new DateTime(2002, 01, 05), calendarResult.Dates[4].Date);
            Assert.Empty(calendarResult.Dates[4].Bookings);
            Assert.Single(calendarResult.Dates[4].PreparationTimes);
        }
    }
}
