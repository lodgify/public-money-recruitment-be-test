using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Data;
using VacationRental.Services.Models;
using VacationRental.Services.Models.Booking;
using VacationRental.Services.Models.Calendar;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Api.Tests
{
    public class VacationRentalWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<VacationRentalObjectContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<VacationRentalObjectContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestVacationRentalDB");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<VacationRentalObjectContext>())
                {
                    try
                    {
                        appContext.Database.EnsureCreated();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            });


        }

        #region BookingsController

        public async Task<Response<BookingDto>> GetBookingAsync(int bookingId)
        {
            var requestUri = $"/api/v1/bookings/{bookingId}";

            Response<BookingDto> result = new Response<BookingDto>();

            using (var client = CreateHttpClientAsync())
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    result.Error = errorInfo;
                }

                result.Content = await response.Content.ReadFromJsonAsync<BookingDto>();
            }

            return result;
        }

        public async Task<Response<BookingDto>> AddBookingAsync(CreateBookingRequest bookingParameters)
        {
            const string requestUri = "/api/v1/bookings";

            Response<BookingDto> result = new Response<BookingDto>();

            using (var client = CreateHttpClientAsync())
            {
                var response = await client.PostAsJsonAsync(requestUri, bookingParameters);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    result.Error = errorInfo;
                }

                result.Content = await response.Content.ReadFromJsonAsync<BookingDto>();
            }

            return result!;
        }

        #endregion

        #region CalendarController

        public async Task<Response<CalendarDto>> GetCalendarAsync(int rentalId, DateTime start, int nights)
        {
            var requestUri = $"/api/v1/calendar?rentalId={rentalId}&start={start.Date}&nights={nights}";

            Response<CalendarDto> result = new Response<CalendarDto>();

            using (var client = CreateHttpClientAsync())
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    result.Error = errorInfo;
                }

                result.Content = await response.Content.ReadFromJsonAsync<CalendarDto>();
            }

            return result!;
        }

        #endregion

        #region RentalsController

        public async Task<Response<RentalDto>> GetRentalByAsync(int rentalId)
        {
            var requestUri = $"/api/v1/rentals/{rentalId}";

            Response<RentalDto> result = new Response<RentalDto>();

            using (var client = CreateHttpClientAsync())
            {
                var response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    result.Error = errorInfo;
                }

                result.Content = await response.Content.ReadFromJsonAsync<RentalDto>();
            }

            return result!;
        }

        public async Task<Response<RentalDto>> AddRentalAsync(CreateRentalRequest rentalParameters)
        {
            const string requestUri = "/api/v1/rentals";

            Response<RentalDto> result = new Response<RentalDto>();

            using (var client = CreateHttpClientAsync())
            {
                var response = await client.PostAsJsonAsync(requestUri, rentalParameters);

                if (!response.IsSuccessStatusCode)
                {
                    var errorInfo = await response.Content.ReadFromJsonAsync<ErrorInfoDto>();
                    result.Error = errorInfo;
                }

                result.Content = await response.Content.ReadFromJsonAsync<RentalDto>();
            }

            return result!;
        }

        #endregion

        #region Private Methods

        private HttpClient CreateHttpClientAsync()
        {
            const string baseAddress = "http://localhost";

            var client = Server.CreateClient();
            client.BaseAddress = new Uri(baseAddress);

            return client;
        }

        #endregion
    }
}
