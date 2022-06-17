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
            const string defaultScheme = "Test Scheme";

            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(servicesConfiguration =>
            {
                servicesConfiguration.AddAuthentication(configure =>
                {
                    configure.DefaultAuthenticateScheme = defaultScheme;
                    configure.DefaultChallengeScheme = defaultScheme;
                }).AddTestAuth(configure => { });
            });
        }

        public async Task<BaseEntityDto> AddRentalAsync(RentalParameters rentalParameters)
        {
            const string requestUri = "/api/v1/rentals";

            BaseEntityDto? result;

            using (var client = await CreateHttpClientAsync(hasAuthorization: true))
            {
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

        public async Task<RentalDto> GetRentalAsync(int rentalId)
        {
            var requestUri = $"/api/v1/rentals/{rentalId}";

            RentalDto? result;

            using (var client = await CreateHttpClientAsync(hasAuthorization: true))
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<RentalDto>();
            }

            return result!;
        }

        public async Task<BaseEntityDto> UpdateRentalAsync(int rentalId, RentalParameters rentalParameters)
        {
            var requestUri = $"/api/v1/rentals/{rentalId}";

            BaseEntityDto? result;

            using (var client = await CreateHttpClientAsync(hasAuthorization: true))
            {
                var response = await client.PutAsJsonAsync(requestUri, rentalParameters);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<BaseEntityDto>();
            }

            return result!;
        }

        public async Task<BaseEntityDto> AddBookingAsync(BookingParameters bookingParameters)
        {
            const string requestUri = "/api/v1/bookings";

            BaseEntityDto? result;

            using (var client = await CreateHttpClientAsync(hasAuthorization: true))
            {
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

        public async Task<CalendarDto> GetCalendarAsync(int rentalId, DateTime start, int nights)
        {
            var requestUri = $"/api/v1/calendar?rentalId={rentalId}&start={start.Date}&nights={nights}";

            CalendarDto? result;

            using (var client = await CreateHttpClientAsync(hasAuthorization: true))
            {
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

        public async Task<BookingDto> GetBookingAsync(int bookingId)
        {
            var requestUri = $"/api/v1/bookings/{bookingId}";

            BookingDto? result;

            using (var client = await CreateHttpClientAsync(hasAuthorization: true))
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<BookingDto>();
            }

            return result!;
        }

        private async Task<AccessTokenDto> GetGuestTokenAsync()
        {
            const string requestUri = "/api/v1/accounts/login-guest";

            AccessTokenDto? result;

            using (var client = await CreateHttpClientAsync())
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    throw new ApplicationException(errorInfo?.Message);
                }

                result = await response.Content.ReadFromJsonAsync<AccessTokenDto>();
            }

            return result!;
        }

        private async Task<HttpClient> CreateHttpClientAsync(bool hasAuthorization = false)
        {
            const string headerParamName = "Authorization";
            const string baseAddress = "http://localhost";

            var client = Server.CreateClient();
            client.BaseAddress = new Uri(baseAddress);

            if (hasAuthorization)
            {
                var guestToken = await GetGuestTokenAsync();
                client.DefaultRequestHeaders.Add(headerParamName, $"{guestToken.TokenType} {guestToken.AccessToken}");
            }

            return client;
        }
    }
}
