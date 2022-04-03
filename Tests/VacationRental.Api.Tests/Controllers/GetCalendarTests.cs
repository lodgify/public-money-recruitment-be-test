using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using VacationRental.Api.Tests.ApiRoutes;
using VacationRental.Application.Bookings.Commands.PostBooking;
using VacationRental.Application.Calendars.Queries.GetCalendar;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Application.Rentals.Commands.PostRental;
using Xunit;

namespace VacationRental.Api.Tests.Controllers
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;

        public GetCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync(RentalApiRoute.Post(), postRentalRequest))
            {
                postRentalResponse.IsSuccessStatusCode.Should().BeTrue();
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = new DateTime(2000, 01, 02)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking1Request))
            {
                postBooking1Response.IsSuccessStatusCode.Should().BeTrue();
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 03)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync(BookingApiRoute.Post(), postBooking2Request))
            {
                postBooking2Response.IsSuccessStatusCode.Should().BeTrue();
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync(CalendarApiRoute.Get(postRentalResult.Id, "2000-01-01", 5)))
            {
                getCalendarResponse.IsSuccessStatusCode.Should().BeTrue();

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                getCalendarResult.RentalId.Should().Be(postRentalResult.Id);
                getCalendarResult.Dates.Should().HaveCount(5);

                getCalendarResult.Dates[0].Date.Should().Be(new DateTime(2000, 01, 01));
                getCalendarResult.Dates[0].Bookings.Should().BeEmpty();

                getCalendarResult.Dates[1].Date.Should().Be(new DateTime(2000, 01, 02));
                getCalendarResult.Dates[1].Bookings.Should().ContainSingle();
                getCalendarResult.Dates[1].Bookings.Should().Contain(x => x.Id == postBooking1Result.Id);

                getCalendarResult.Dates[2].Date.Should().Be(new DateTime(2000, 01, 03));
                getCalendarResult.Dates[2].Bookings.Should().HaveCount(2);
                getCalendarResult.Dates[2].Bookings.Should().Contain(x => x.Id == postBooking1Result.Id);

                getCalendarResult.Dates[3].Date.Should().Be(new DateTime(2000, 01, 04));
                getCalendarResult.Dates[3].Bookings.Should().ContainSingle();
                getCalendarResult.Dates[3].PreparationTime.Should().ContainSingle();
                getCalendarResult.Dates[3].Bookings.Should().Contain(x => x.Id == postBooking2Result.Id);

                getCalendarResult.Dates[4].Date.Should().Be(new DateTime(2000, 01, 05));
                getCalendarResult.Dates[4].PreparationTime.Should().ContainSingle();
                getCalendarResult.Dates[4].Bookings.Should().BeEmpty();
            }
        }
    }
}
