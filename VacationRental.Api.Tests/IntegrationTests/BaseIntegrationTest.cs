using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace VacationRental.Api.Tests.IntegrationTests
{
    public class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> _factory;

        protected readonly string _rentalUrl = "/api/v1/Rental/";
        protected readonly string _bookingUrl = "/api/v1/Booking/";

        public HttpClient Client { get; }

        public BaseIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            Client = _factory.CreateClient();
        }
    }
}
