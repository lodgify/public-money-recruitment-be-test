using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Application;
using VacationRental.Application.Bookings.Commands.CreateBooking;
using VacationRental.Application.Rentals.Commands.CreateRental;

namespace VacationRental.Api.IntegrationTests
{
    public class VacationRentalApiCaller
    {
        private readonly HttpClient _client;

        public VacationRentalApiCaller(VacationRentalWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        public async Task<ResourceIdViewModel> PostRental(int units, int preparationTimeInDays)
        {
            var command = new CreateRentalCommand
            {
                Units = units,
                PreparationTimeInDays = preparationTimeInDays,
            };

            var response = await PostAsync($"/api/v1/rentals", command);
            return await response.Content.ReadAsAsync<ResourceIdViewModel>();
        }

        public async Task<ResourceIdViewModel> PostBooking(int rentalId, int nights, DateTime start)
        {
            var command = new CreateBookingCommand
            {
                RentalId = rentalId,
                Nights = nights,
                Start = start
            };

            var response = await PostAsync($"/api/v1/bookings", command);
            return await response.Content.ReadAsAsync<ResourceIdViewModel>();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            return await _client.SendAsync(httpRequest);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object payload)
        {
            using var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, MediaTypeNames.Application.Json);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = stringContent
            };

            return await _client.SendAsync(httpRequest);
        }
    }
}
