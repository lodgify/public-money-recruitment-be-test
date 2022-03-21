using Autofac.Extensions.DependencyInjection;
//using Lodgify.VacationRentalService.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using VacationRental.WebAPI;
using VacationRental.WebAPI.Configurations;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
	[CollectionDefinition("Integration")]
	public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
	{
		private readonly IHost _host;
		private readonly TestServer _server;
		private readonly HttpClient _client;

		public Request Request { get; }
		public ServiceConfiguration Configuration { get; }


		public IntegrationFixture()
		{
			var hostBuilder = new HostBuilder()
						.ConfigureWebHost(webHost =>
						{
							webHost.UseTestServer();
							webHost.UseStartup<Startup>();
						})
						.UseServiceProviderFactory(new AutofacServiceProviderFactory());

			var configBuilder = new ConfigurationBuilder()
						.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			var configRoot = configBuilder.Build();
			Configuration = configRoot.GetSection("VacationRentalServiceConfiguration").Get<ServiceConfiguration>();

			_host = hostBuilder.Start();
			_server = _host.GetTestServer();
			_client = _server.CreateClient();

			Request = new Request(_client);
		}

		public void Dispose()
		{
			_client.Dispose();
			_server.Dispose();
		}
	}
}
