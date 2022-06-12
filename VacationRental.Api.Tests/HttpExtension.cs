using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    public static class HttpExtension
    {

        public static async Task<ResourceIdViewModel> PostBookingAndAssertSuccess(this HttpClient client, BookingBindingModel postBookingRequest)
        {
            ResourceIdViewModel result;
            using (var postBooking1Response = await client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            return result;
        }

        public static async Task<ResourceIdViewModel> PostVacationRentalAndAssertSuccess(this HttpClient client, RentalViewModel rentalRequest)
        {
            ResourceIdViewModel postResult;
            using (var postResponse = await client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", rentalRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }
            return postResult;
        }

        public static async Task<ResourceIdViewModel> PostRentalAndAssertSuccess(this HttpClient client, RentalBindingModel rentalRequest)
        {
            ResourceIdViewModel postResult;
            using (var postResponse = await client.PostAsJsonAsync($"/api/v1/rentals", rentalRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }
            return postResult;
        }
    }
}
