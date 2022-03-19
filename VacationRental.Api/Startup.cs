using Autofac;
using FluentValidation.AspNetCore;
//using VacationRental.Configurations;
using VacationRental.WebAPI.Configurations;
using VacationRental.WebAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using VacationRental.SqlDataAccess;

namespace VacationRental.WebAPI
{
	public class Startup
	{
		private readonly IConfiguration configuration;
		private string binPath;
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="env"> Host environment, will be used to determine the service configuration </param>
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
						 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
						 .AddEnvironmentVariables();
			binPath = System.IO.Directory.GetParent(typeof(Program).Assembly.Location).FullName;
			configuration = builder.Build();
		}

		/// <summary>
		/// Use this method to add services into the container.
		/// </summary>
		/// <param name="services"> IServiceCollection used for the registration of services </param>
		public void ConfigureServices(IServiceCollection services)
		{
			var serviceConfiguration = configuration.GetSection("VacationRentalServiceConfiguration").Get<ServiceConfiguration>();
			services.AddSingleton(serviceConfiguration);
			services.AddMvc()
					.AddFluentValidation(mvcConfig => mvcConfig.RegisterValidatorsFromAssemblyContaining<Startup>())
					.AddNewtonsoftJson();

			//Add Swagger documentation, this will let the developers see the API documentation in an intertactive web application
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Lodgify Vacation Rental Task",
					Version = "v1",
					Description = "Vacation rental service to manage bookings and rentals"
				});
			});

			//Add automapper service, this will let developers define DTO <-> Model conversion in a scalable way.
			services.AddAutoMapper(typeof(Startup));

			services.AddControllers();

			//Add DbContext using SQLite
			services.AddDbContext<DatabaseContext>((serviceProvider, optionsBuilder) =>
			{
				var dbSourceText = "Data Source=";
				var dbName = serviceConfiguration.DatabaseConnection.Replace(dbSourceText, "", StringComparison.OrdinalIgnoreCase);
				var databaseLocation = System.IO.Path.Combine(this.binPath, dbName);
				optionsBuilder.UseSqlite($"{dbSourceText}{databaseLocation}");

			}, ServiceLifetime.Transient);

			services.Configure<KestrelServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule(new Module.WebAPI());
		}

		/// <summary>
		/// This method configures the HTTP request pipeline and further configurations.
		/// </summary>
		/// <param name="app"> Application builder used to customize the request pipeline </param>
		/// <param name="env"> Host environment will determine how to initialize the DB </param>
		/// <param name="dbContext"> The database context, will be used to initialize the DB </param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext dbContext)
		{
			//Check db integrity
			dbContext.Database.EnsureCreated();

			//Middleware
			if (env.IsEnvironment("Debug"))
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseMiddleware<HttpExceptionHandler>();

			app.UseRouting();
			app.UseEndpoints(e => e.MapControllers());

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lodgify");
			});
		}
	}
}
