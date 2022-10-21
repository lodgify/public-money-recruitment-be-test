using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace VacationRental.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog((ctx, lc) =>
                {
                    lc.ReadFrom.Configuration(ctx.Configuration);
                })
                .UseStartup<Startup>();
    }
}
