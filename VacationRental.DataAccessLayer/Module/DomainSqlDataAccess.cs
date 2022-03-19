using Autofac;
using VacationRental.Domain;
using VacationRental.Domain.Models;

namespace VacationRental.SqlDataAccess.Module
{
	public class DomainSqlDataAccess : Autofac.Module
	{
		/// Overriden Load function to add registrations to the container.
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EFCRepository<Booking>>().As<GIRepository<Booking>>();
			builder.RegisterType<EFCRepository<Rental>>().As<GIRepository<Rental>>();
		}
	}
}
