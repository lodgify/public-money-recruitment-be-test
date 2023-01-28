namespace VacationRental.Application.Dtos
{
	public class RentalDto
	{
		public RentalDto(int units, int preparationTimeInDays)
		{
			this.Units = units;
			this.PreparationTimeInDays = preparationTimeInDays;
		}

		public int Units { get; set; }

		public int PreparationTimeInDays { get; set; }
	}
}
