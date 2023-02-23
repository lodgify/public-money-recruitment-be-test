using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Models.ViewModels.Rental;
using Xunit;

namespace VacationRental.Api.Tests.IntegrationTests;

[Collection("Integration")]
public sealed class IntegrationRentalTests : BaseIntegrationTest
{
    public IntegrationRentalTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ReturnsTheCreatedRentalWhenPostRental()
    {
        var request = new RentalBindingModel
        {
            Units = 2,
            PreparationTimeInDays = 1
        };

        using var postResponse = await Client.PostAsJsonAsync(_rentalUrl, request);

        Assert.True(postResponse.IsSuccessStatusCode);
        var postResult = await postResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();

        using var getResponse = await Client.GetAsync(_rentalUrl + postResult.Id);

        Assert.True(getResponse.IsSuccessStatusCode);

        var getResult = await getResponse.Content.ReadFromJsonAsync<RentalViewModel>();
        Assert.Equal(request.Units, getResult.Units);
    }

    [Fact]
    public async Task UpdatesRentalWhenPutRental()
    {
        var postRequest = new RentalBindingModel
        {
            Units = 2,
            PreparationTimeInDays = 1
        };
        var putRequest = new RentalBindingModel
        {
            Units = 4,
            PreparationTimeInDays = 2
        };
        using var postResponse = await Client.PostAsJsonAsync(_rentalUrl, postRequest);
        Assert.True(postResponse.IsSuccessStatusCode);
        var postResult = await postResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();

        using var putResponse = await Client.PutAsJsonAsync(_rentalUrl + postResult.Id, putRequest);
        Assert.True(putResponse.IsSuccessStatusCode);
        var putResult = await putResponse.Content.ReadFromJsonAsync<UpdateRentalViewModel>();

        using var getResponse = await Client.GetAsync(_rentalUrl + postResult.Id);
        Assert.True(getResponse.IsSuccessStatusCode);

        var getResult = await getResponse.Content.ReadFromJsonAsync<RentalViewModel>();
        Assert.Equal(putResult.Units, getResult.Units);
    }

    [Fact]
    public async Task RetursBadRequestWhenInvalidPostRental()
    {
        var request = new RentalBindingModel
        {
            Units = 0
        };

        ResourceIdViewModel? postResult;
        using var postResponse = await Client.PostAsJsonAsync(_rentalUrl, request);

        Assert.True(!postResponse.IsSuccessStatusCode);
        Assert.Equal(postResponse.StatusCode, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RetursNotFoundWhenInvalidPostRental()
    {
        using var getResponseBadRequest = await Client.GetAsync(_rentalUrl + int.MinValue);
        Assert.True(!getResponseBadRequest.IsSuccessStatusCode);
        Assert.Equal(getResponseBadRequest.StatusCode, HttpStatusCode.BadRequest);

        using var getResponseNotFound = await Client.GetAsync(_rentalUrl + int.MaxValue);
        Assert.True(!getResponseNotFound.IsSuccessStatusCode);
        Assert.Equal(getResponseNotFound.StatusCode, HttpStatusCode.NotFound);
    }
}
