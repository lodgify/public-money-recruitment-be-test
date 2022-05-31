namespace VacationRental.Api.Models
{
	public class CalendarBookingViewModel
	{
		#region Contructors
		public CalendarBookingViewModel() { }

		private CalendarBookingViewModel(int id, int unit)
		{
			Id = id;
			Unit = unit;
		}
		#endregion Contructors

		#region Properties
		public int Id { get; set; }
		public int Unit { get; set; }
		#endregion Properties

		#region Static methods
		public static CalendarBookingViewModel Create(int id, int unit = 1)
		{
			CalendarBookingViewModel result = new CalendarBookingViewModel(id, unit);
			return result;
		}
		#endregion

	}
}
