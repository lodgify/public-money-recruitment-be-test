﻿using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using Xunit;

namespace VacationRental.Api.Tests.Integration.Rentals;

[Collection("Integration")]
public class PostRentalTests
{
    private readonly HttpClient _client;

    public PostRentalTests(IntegrationFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
    {
        var request = new RentalBindingModel
        {
            Units = 25
        };

        int postResult;
        using var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request);
        Assert.True(postResponse.IsSuccessStatusCode);
        postResult = await postResponse.Content.ReadAsAsync<int>();

        using var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult}");

        Assert.True(getResponse.IsSuccessStatusCode);

        var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
        Assert.Equal(request.Units, getResult.Units);
    }
}