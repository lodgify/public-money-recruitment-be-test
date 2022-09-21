﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using Xunit;

namespace VacationRental.Api.Tests.Integration.Calendar;

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
            Units = 2
        };

        int postRentalResult;
        using var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest);

        Assert.True(postRentalResponse.IsSuccessStatusCode);
        postRentalResult = await postRentalResponse.Content.ReadAsAsync<int>();

        var postBooking1Request = new BookingBindingModel
        {
            RentalId = postRentalResult,
            Nights = 2,
            Start = new DateTime(2000, 01, 02),
            Unit = 1
        };

        int postBooking1Result;
        using var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request);

        Assert.True(postBooking1Response.IsSuccessStatusCode);
        postBooking1Result = await postBooking1Response.Content.ReadAsAsync<int>();

        var postBooking2Request = new BookingBindingModel
        {
            RentalId = postRentalResult,
            Nights = 2,
            Start = new DateTime(2000, 01, 03),
            Unit = 2
        };

        int postBooking2Result;
        using var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request);
        Assert.True(postBooking2Response.IsSuccessStatusCode, "1");
        postBooking2Result = await postBooking2Response.Content.ReadAsAsync<int>();


        using var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult}&start=2000-01-01&nights=5");

        Assert.True(getCalendarResponse.IsSuccessStatusCode, "2");

        var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

        Assert.Equal(postRentalResult, getCalendarResult.RentalId);
        Assert.Equal(5, getCalendarResult.Dates.Count);

        Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
        Assert.Empty(getCalendarResult.Dates[0].Bookings);

        Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
        Assert.Single(getCalendarResult.Dates[1].Bookings);
        Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result);

        Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
        Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
        Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result);
        Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result);

        Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
        Assert.Single(getCalendarResult.Dates[3].Bookings);
        Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result);

        Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
        Assert.Empty(getCalendarResult.Dates[4].Bookings);
    }
}
