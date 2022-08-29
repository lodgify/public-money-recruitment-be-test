using System;
using System.Threading.Tasks;
using VacationRental.Services.Models.Booking;
using VacationRental.Services.Models.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests : IClassFixture<VacationRentalWebApplicationFactory<Program>>
    {
        private readonly VacationRentalWebApplicationFactory<Program> _applicationFactory;

        public GetCalendarTests(VacationRentalWebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new CreateRentalRequest
            {
                Units = 2,
                PreparationTime = 2
            };

            var rentalResult = await _applicationFactory.AddRentalAsync(postRentalRequest);
            Assert.Null(rentalResult.Error);
            var postBooking1Request = new CreateBookingRequest
            {
                 RentalId = rentalResult.Content.Id,
                 Nights = 2,
                 Start = new DateTime(2002, 01, 02)
            };

            var postBooking1Result = await _applicationFactory.AddBookingAsync(postBooking1Request);
            Assert.Null(postBooking1Result.Error);
            var postBooking2Request = new CreateBookingRequest
            {
                RentalId = rentalResult.Content.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 03)
            };

            var postBooking2Result = await _applicationFactory.AddBookingAsync(postBooking2Request);
            Assert.Null(postBooking2Result.Error);

            var start = new DateTime(2002, 01, 01);
            const int nights = 5;

            var calendarResult = await _applicationFactory.GetCalendarAsync(rentalResult.Content.Id, start, nights);

            Assert.Equal(rentalResult.Content.Id, calendarResult.Content.RentalId);
            Assert.Equal(nights, calendarResult.Content.Dates?.Count);

            Assert.Equal(new DateTime(2002, 01, 01), calendarResult.Content.Dates[0].Date);
            Assert.Empty(calendarResult.Content.Dates[0].Bookings);

            Assert.Equal(new DateTime(2002, 01, 02), calendarResult.Content.Dates[1].Date);
            Assert.Single(calendarResult.Content.Dates[1].Bookings);
            Assert.Contains(calendarResult.Content.Dates[1].Bookings, x => x.Id == postBooking1Result.Content.Id);

            Assert.Equal(new DateTime(2002, 01, 03), calendarResult.Content.Dates[2].Date);
            Assert.Equal(1, calendarResult.Content.Dates[2].Bookings?.Count);
            Assert.Contains(calendarResult.Content.Dates[2].Bookings, x => x.Id == postBooking2Result.Content.Id);

            Assert.Equal(new DateTime(2002, 01, 04), calendarResult.Content.Dates[3].Date);
            Assert.Empty(calendarResult.Content.Dates[3].Bookings);
            Assert.Single(calendarResult.Content.Dates[3].PreparationTimes);

            Assert.Equal(new DateTime(2002, 01, 05), calendarResult.Content.Dates[4].Date);
            Assert.Empty(calendarResult.Content.Dates[4].Bookings);
            Assert.Single(calendarResult.Content.Dates[4].PreparationTimes);
        }
    }
}
