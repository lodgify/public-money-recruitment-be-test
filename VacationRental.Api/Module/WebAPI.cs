using Autofac;
using VacationRental.SqlDataAccess.Module;

namespace VacationRental.WebAPI.Module
{
	/// <summary>
	/// This class registers other modules used in the service into the autofac IoC container.
	/// </summary>
	public class WebAPI : Autofac.Module
	{
		/// <summary>
		/// Adds registrations to the WebAPI module.
		/// Registers the service's domain module and the DomainSqlDataAccess module
		/// </summary>
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule(new Domain());
			builder.RegisterModule(new DomainSqlDataAccess());
		}
	}
}
