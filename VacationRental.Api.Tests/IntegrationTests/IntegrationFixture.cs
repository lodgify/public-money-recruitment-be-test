using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace VacationRental.Api.Tests.IntegrationTests
{
    public sealed class IntegrationFixture : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HttpClient Client { get; }

        public IntegrationFixture(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            Client = _factory.CreateClient();
        }


        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/About")]
        [InlineData("/Privacy")]
        [InlineData("/Contact")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
