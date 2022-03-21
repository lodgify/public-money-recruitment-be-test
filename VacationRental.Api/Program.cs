using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace VacationRental.WebAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

			try
			{
				logger.Info("Lodgify VacationRental Service Init");

				var host = Host.CreateDefaultBuilder(args)
							   .UseServiceProviderFactory(new AutofacServiceProviderFactory())
							   .ConfigureWebHostDefaults(webHostBuilder =>
							   {
								   webHostBuilder.ConfigureLogging(l => l.ClearProviders())
												 .ConfigureLogging(l => l.SetMinimumLevel(LogLevel.Information))
												 .ConfigureLogging(l => l.AddConsole())
												 .UseNLog()
												 .UseKestrel()
												 .UseStartup<Startup>()
												 .UseUrls("http://0.0.0.0:9981/");

							   })
							   .Build();

				await host.RunAsync();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}
	}
}
