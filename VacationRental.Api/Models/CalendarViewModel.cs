using System.Collections.Generic;

namespace VacationRental.Api.Models
{
	public class CalendarViewModel
	{
		#region Contructors
		public CalendarViewModel() { }

		private CalendarViewModel(int rentalId, List<CalendarDateViewModel> dates)
		{
			RentalId = rentalId;
			Dates = dates;
		}
		#endregion Contructors

		#region Properties
		public int RentalId { get; set; }
		public List<CalendarDateViewModel> Dates { get; set; }
		#endregion Properties

		#region Static methods
		public static CalendarViewModel Create(int rentalId, List<CalendarDateViewModel> dates)
		{
			CalendarViewModel result = new CalendarViewModel(rentalId, dates);
			return result;
		}
		#endregion Static methods
	}
}
