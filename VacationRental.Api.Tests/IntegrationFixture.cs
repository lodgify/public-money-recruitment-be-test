using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace VacationRental.Api.Tests
{
    public sealed class IntegrationFixture : WebApplicationFactory<Program>
    {
        public HttpClient Client { get; }

        public IntegrationFixture()
        {
            var webAppFactory = new WebApplicationFactory<Program>();

            Client = webAppFactory.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}