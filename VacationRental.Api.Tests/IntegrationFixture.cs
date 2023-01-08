using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using System.ComponentModel.Design;
using System.Net.Http;
using Xunit;

namespace VacationRental.Api.Tests
{
    [CollectionDefinition("Integration")]
    public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
    {
        private readonly WebApplicationFactory<Program> _application;

        public HttpClient Client { get; }

        public IntegrationFixture()
        {
            _application = new WebApplicationFactory<Program>();                

            Client = _application.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _application.Dispose();
        }
    }
}
