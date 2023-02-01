using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests;

public abstract class BaseIntegrationTests
{
    protected readonly HttpClient Client;

    protected BaseIntegrationTests(IntegrationFixture fixture)
    {
        Client = fixture.Client;
    }

    protected async Task<int> CreateRentalAsync(int units)
    {
        var request = new RentalBindingModel
        {
            Units = units
        };

        using var response = await Client.PostAsJsonAsync($"/api/v1/rentals", request);
        Assert.True(response.IsSuccessStatusCode);
        var result= await response.Content.ReadAsAsync<ResourceIdViewModel>();
        
        return result.Id;
    }
    
    protected async Task<int> CreateVacationRentalAsync(int units, int preparationTime)
    {
        var request = new VacationRentalBindingModel
        {
            Units = units,
            PreparationTimeInDays = preparationTime
        };

        using var response = await Client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", request);
        Assert.True(response.IsSuccessStatusCode);
        var result= await response.Content.ReadAsAsync<ResourceIdViewModel>();
        
        return result.Id;
    }
    
    protected async Task<int> CreateBookingAsync(int rentalId, DateTime start, int nights)
    {
        var request = new BookingBindingModel
        {
            RentalId = rentalId,
            Start = start,
            Nights = nights
        };

        using var response = await Client.PostAsJsonAsync($"/api/v1/bookings", request);
        Assert.True(response.IsSuccessStatusCode);
        var result= await response.Content.ReadAsAsync<ResourceIdViewModel>();
        
        return result.Id;
    }
}