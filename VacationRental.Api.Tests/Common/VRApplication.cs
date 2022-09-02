using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VR.Application.Queries.GetBooking;
using VR.Application.Queries.GetCalendar;
using VR.Application.Queries.GetRental;
using VR.Application.Requests.AddBooking;
using VR.Application.Requests.AddRental;
using VR.Application.Requests.UpdateRental;
using VR.DataAccess;

namespace VacationRental.Api.Tests.Common
{
    internal class VRApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<VRContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<VRContext>
                    (options =>
                    {
                        options.UseInMemoryDatabase("TestingDb");
                    });
            });
        }

        public async Task<AddRentalResponse> AddRentalAsync(AddRentalRequest request)
        {
            const string requestUri = "/api/v1/rentals";

            AddRentalResponse result;

            using (var client = await CreateHttpClientAsync())
            {
                var response = await client.PostAsJsonAsync(requestUri, request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException();
                }

                result = await response.Content.ReadFromJsonAsync<AddRentalResponse>();
            }

            return result;
        }

        public async Task<GetRentalResponse> GetRentalAsync(int rentalId)
        {
            var requestUri = $"/api/v1/rentals/{rentalId}";

            GetRentalResponse result;

            using (var client = await CreateHttpClientAsync())
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<Exception>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<GetRentalResponse>();
            }

            return result;
        }

        public async Task<HttpResponseMessage> UpdateRentalAsync(int rentalId, UpdateRentalRequest request)
        {
            var requestUri = $"/api/v1/rentals/{rentalId}";

            UpdateRentalResponse result;
            HttpResponseMessage response;

            using (var client = await CreateHttpClientAsync())
            {
                response = await client.PutAsJsonAsync(requestUri, request);
            }

            return response;
        }

        public async Task<AddBookingResponse> AddBookingAsync(AddBookingRequest request)
        {
            const string requestUri = "/api/v1/bookings";

            AddBookingResponse result;

            using (var client = await CreateHttpClientAsync())
            {
                var response = await client.PostAsJsonAsync(requestUri, request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<Exception>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<AddBookingResponse>();
            }

            return result;
        }

        public async Task<GetBookingResponse> GetBookingAsync(int bookingId)
        {
            var requestUri = $"/api/v1/bookings/{bookingId}";

            GetBookingResponse result;

            using (var client = await CreateHttpClientAsync())
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<Exception>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<GetBookingResponse>();
            }

            return result;
        }

        public async Task<GetCalendarResponse> GetCalendarAsync(int rentalId, DateTime start, int nights)
        {
            var requestUri = $"/api/v1/calendar?rentalId={rentalId}&start={start.Date}&nights={nights}";

            GetCalendarResponse result;

            using (var client = await CreateHttpClientAsync())
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<Exception>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<GetCalendarResponse>();
            }

            return result;
        }

        public async Task<HttpResponseMessage> GetHttpRequestAsync(string requestUri)
        {
            HttpResponseMessage response = null;

            using (var client = await CreateHttpClientAsync())
            {
                response = await client.GetAsync(requestUri);
            }

            return response;
        }

        public async Task<HttpResponseMessage> PostHttpRequestAsync(string requestUri, object body)
        {
            HttpResponseMessage response = null;

            using (var client = await CreateHttpClientAsync())
            {
                response = await client.PostAsJsonAsync(requestUri, body);
            }

            return response;
        }

        private async Task<HttpClient> CreateHttpClientAsync()
        {
            const string baseAddress = "http://localhost";

            var client = Server.CreateClient();
            client.BaseAddress = new Uri(baseAddress);

            return client;
        }
    }
}
