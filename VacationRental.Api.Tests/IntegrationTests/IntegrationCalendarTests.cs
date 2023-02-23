using Microsoft.AspNetCore.Mvc.Testing;
using Models.ViewModels.Booking;
using Models.ViewModels.Calendar;
using Models.ViewModels.Rental;
using System.Net.Http.Json;
using Xunit;

namespace VacationRental.Api.Tests.IntegrationTests;

[Collection("Integration")]
public sealed class IntegrationCalendarTests : BaseIntegrationTest
{
    public IntegrationCalendarTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ReturnsTheCalculatedCalendarWhenGetCalendar()
    {
        var postRentalRequest = new RentalBindingModel
        {
            Units = 2,
            PreparationTimeInDays = 1
        };

        using var postRentalResponse = await Client.PostAsJsonAsync(_rentalUrl, postRentalRequest);
        Assert.True(postRentalResponse.IsSuccessStatusCode);
        var postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();

        var postBooking1Request = new BookingBindingViewModel
        {
            RentalId = postRentalResult.Id,
            Nights = 2,
            Start = new DateTime(2000, 01, 02)
        };

        using var postBooking1Response = await Client.PostAsJsonAsync(_bookingUrl, postBooking1Request);
        Assert.True(postBooking1Response.IsSuccessStatusCode);
        var postBooking1Result = await postBooking1Response.Content.ReadFromJsonAsync<ResourceIdViewModel>();

        var postBooking2Request = new BookingBindingViewModel
        {
            RentalId = postRentalResult.Id,
            Nights = 2,
            Start = new DateTime(2000, 01, 03)
        };

        using var postBooking2Response = await Client.PostAsJsonAsync(_bookingUrl, postBooking2Request);
        Assert.True(postBooking2Response.IsSuccessStatusCode);
        var postBooking2Result = await postBooking2Response.Content.ReadFromJsonAsync<ResourceIdViewModel>();

        using var getCalendarResponse = await Client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=5");
        Assert.True(getCalendarResponse.IsSuccessStatusCode);

        var getCalendarResult = await getCalendarResponse.Content.ReadFromJsonAsync<CalendarViewModel>();

        Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
        Assert.Equal(5, getCalendarResult.Dates.Count);

        Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
        Assert.Empty(getCalendarResult.Dates[0].Bookings);

        Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
        Assert.Single(getCalendarResult.Dates[1].Bookings);
        Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

        Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
        Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
        Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
        Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

        Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
        Assert.Single(getCalendarResult.Dates[3].Bookings);
        Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

        Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
        Assert.Empty(getCalendarResult.Dates[4].Bookings);
    }
}
