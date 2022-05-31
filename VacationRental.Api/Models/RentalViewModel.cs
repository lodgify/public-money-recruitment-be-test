namespace VacationRental.Api.Models
{
	public class RentalViewModel
	{
		#region Contructors
		public RentalViewModel() { }

		private RentalViewModel(int id, int units, int preparationTimeInDays)
		{
			Id = id;
			Units = units;
			PreparationTimeInDays = preparationTimeInDays;
		}
		private RentalViewModel(int units, int preparationTimeInDays)
		{
			Units = units;
			PreparationTimeInDays = preparationTimeInDays;
		}
		#endregion Contructors

		#region Properties
		public int Id { get; set; }
		public int Units { get; set; }
		public int PreparationTimeInDays { get; set; }
		#endregion Properties

		#region Static methods
		public static RentalViewModel Create(int id, int units, int preparationTimeInDays)
		{
			RentalViewModel result = new RentalViewModel(id, units, preparationTimeInDays);
			return result;
		}

		public static RentalViewModel Create(int units, int preparationTimeInDays)
		{
			RentalViewModel result = new RentalViewModel(units, preparationTimeInDays);
			return result;
		}

		#endregion Static methods
	}
}
