using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Api.Models
{
	public class PreparationTimeViewModel
	{
		#region Contructors
		public PreparationTimeViewModel() { }

		private PreparationTimeViewModel(int unit)
		{
			Unit = unit;
		}
		#endregion Contructors

		#region Properties
		public int Unit { get; set; }
		#endregion Properties

		#region Static methods
		public static PreparationTimeViewModel Create(int unit = 1)
		{
			PreparationTimeViewModel result = new PreparationTimeViewModel(unit);
			return result;
		}
		#endregion Static methods
	}
}
