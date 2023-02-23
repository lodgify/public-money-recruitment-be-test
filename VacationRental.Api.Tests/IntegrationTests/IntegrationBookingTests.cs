using Microsoft.AspNetCore.Mvc.Testing;
using Models.ViewModels.Booking;
using Models.ViewModels.Rental;
using System.Net.Http.Json;
using Xunit;

namespace VacationRental.Api.Tests.IntegrationTests;

[Collection("Integration")]
public sealed class IntegrationBookingTests : BaseIntegrationTest
{
    public IntegrationBookingTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ReturnsTheCreatedBookingWhenPostBooking()
    {
        var postRentalRequest = new RentalBindingModel
        {
            Units = 4,
            PreparationTimeInDays = 1
        };

        using var postRentalResponse = await Client.PostAsJsonAsync(_rentalUrl, postRentalRequest);
        Assert.True(postRentalResponse.IsSuccessStatusCode);
        var postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel?>();

        var postBookingRequest = new BookingBindingViewModel
        {
            RentalId = postRentalResult.Id,
            Nights = 3,
            Start = new DateTime(2001, 01, 01)
        };

        using var postBookingResponse = await Client.PostAsJsonAsync(_bookingUrl, postBookingRequest);
        Assert.True(postBookingResponse.IsSuccessStatusCode);
        var postBookingResult = await postBookingResponse.Content.ReadFromJsonAsync<ResourceIdViewModel?>();

        using var getBookingResponse = await Client.GetAsync(_bookingUrl + postBookingResult.Id);
        Assert.True(getBookingResponse.IsSuccessStatusCode);

        var getBookingResult = await getBookingResponse.Content.ReadFromJsonAsync<BookingViewModel>();
        Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
        Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
        Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
    }

    [Fact]
    public async Task ReturnsErrorWhenThereIsOverbooking()
    {
        var postRentalRequest = new RentalBindingModel
        {
            Units = 1,
            PreparationTimeInDays = 1
        };

        using var postRentalResponse = await Client.PostAsJsonAsync(_rentalUrl, postRentalRequest);
        Assert.True(postRentalResponse.IsSuccessStatusCode);
        var postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel?>();

        var postBooking1Request = new BookingBindingViewModel
        {
            RentalId = postRentalResult.Id,
            Nights = 3,
            Start = new DateTime(2002, 01, 01)
        };

        using var postBooking1Response = await Client.PostAsJsonAsync(_bookingUrl, postBooking1Request);
        Assert.True(postBooking1Response.IsSuccessStatusCode);

        var postBooking2Request = new BookingBindingViewModel
        {
            RentalId = postRentalResult.Id,
            Nights = 1,
            Start = new DateTime(2002, 01, 02)
        };

        using var postBooking2Response = await Client.PostAsJsonAsync(_bookingUrl, postBooking2Request);
        Assert.True(!postBooking2Response.IsSuccessStatusCode);
    }


    [Fact]
    public async Task ReturnsErrorWhenBokingNotFound()
    {
        var postRentalRequest = new RentalBindingModel
        {
            Units = 1,
            PreparationTimeInDays = 1
        };

        using var postRentalResponse = await Client.PostAsJsonAsync(_rentalUrl, postRentalRequest);
        Assert.True(postRentalResponse.IsSuccessStatusCode);
        var postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel?>();

        var postBooking1Request = new BookingBindingViewModel
        {
            RentalId = postRentalResult.Id,
            Nights = 3,
            Start = new DateTime(2002, 01, 01)
        };

        using var postBookingResponse = await Client.PostAsJsonAsync(_bookingUrl, postBooking1Request);
        Assert.True(postBookingResponse.IsSuccessStatusCode);
        var postBookingResult = await postBookingResponse.Content.ReadFromJsonAsync<ResourceIdViewModel?>();

        using var postBooking2Response = await Client.GetAsync(_bookingUrl + postBookingResult.Id);
        Assert.True(postBooking2Response.IsSuccessStatusCode);

        using var postBooking3Response = await Client.GetAsync(_bookingUrl + int.MaxValue);
        Assert.True(!postBooking3Response.IsSuccessStatusCode);
    }
}
