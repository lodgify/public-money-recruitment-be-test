using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using VacationRental.Common.Models;
using VacationRental.Core.Mapping;
using VacationRental.Data;
using VacationRental.Repository.Implementations;
using VacationRental.Repository.Interfaces;
using VacationRental.Service.Implementations;
using VacationRental.Service.Interfaces;

namespace VacationRental.Core.DependencyInjection
{
    public class DependencyConfiguration
    {
		public static void Inject(IServiceCollection services, string connectionString = "")
		{
			services.AddDbContext<DataContext>(options =>
				options.UseLazyLoadingProxies().UseNpgsql(connectionString));
			services.AddAutoMapper(typeof(AutoMapping));

			services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
			services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());

			services.AddScoped<IBookingRepository, BookingRepository>();
			services.AddScoped<IRentalRepository, RentalRepository>();

			services.AddScoped<IRentalService, RentalService>();
			services.AddScoped<IBookingService, BookingService>();
		}
	}
}
