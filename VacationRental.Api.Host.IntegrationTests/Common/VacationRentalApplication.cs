using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Json;
using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;

namespace VacationRental.Api.Host.IntegrationTests.Common
{
    internal class VacationRentalApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test Scheme";
                    options.DefaultChallengeScheme = "Test Scheme";
                }).AddTestAuth(o => { });
            });
        }

        public HttpClient CreateHttpClient()
        {
            const string baseAddress = "http://localhost";

            var result = Server.CreateClient();
            result.BaseAddress = new Uri(baseAddress);

            return result;
        }

        public async Task<AccessTokenDto> GetGuestTokenAsync()
        {
            AccessTokenDto? result;

            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"/api/v1/accounts/login-guest");

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<AccessTokenDto>();
            }

            return result!;
        }

        public async Task<BaseEntityDto> AddRentalAsync(string accessToken, RentalParameters rentalParameters)
        {
            const string requestUri = "/api/v1/rentals";

            BaseEntityDto? result;

            using (var client = CreateHttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsJsonAsync(requestUri, rentalParameters);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<BaseEntityDto>();
            }

            return result!;
        }

        public async Task<RentalDto> GetRentalAsync(string accessToken, int rentalId)
        {
            RentalDto? result;

            using (var client = CreateHttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.GetAsync($"/api/v1/rentals/{rentalId}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<RentalDto>();
            }

            return result!;
        }

        public async Task<BaseEntityDto> AddBookingAsync(string accessToken, BookingParameters bookingParameters)
        {
            const string requestUri = "/api/v1/bookings";

            BaseEntityDto? result;

            using (var client = CreateHttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsJsonAsync(requestUri, bookingParameters);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<BaseEntityDto>();
            }

            return result!;
        }

        public async Task<CalendarDto> GetCalendarAsync(string accessToken, int rentalId)
        {
            var requestUri = $"/api/v1/calendar?rentalId={rentalId}&start=2002-01-01&nights=5";

            CalendarDto? result;

            using (var client = CreateHttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<CalendarDto>();
            }

            return result!;
        }

        public async Task<BookingDto> GetBookingAsync(string accessToken, int bookingId)
        {
            BookingDto? result;

            using (var client = CreateHttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.GetAsync($"/api/v1/bookings/{bookingId}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<BookingDto>();
            }

            return result!;
        }
    }
}
