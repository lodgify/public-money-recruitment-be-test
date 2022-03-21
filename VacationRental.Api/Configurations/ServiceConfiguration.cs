using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.WebAPI.Configurations
{
	public class ServiceConfiguration
	{
		/// <summary>
		/// Database connection string
		/// </summary>
		/// <example>"Data Source=lodgify_vacation.db"</example>
		public string DatabaseConnection { get; set; }
	}
}
