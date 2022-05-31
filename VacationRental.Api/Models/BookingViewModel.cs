using System;

namespace VacationRental.Api.Models
{
	public class BookingViewModel
	{
		#region Constructors
		public BookingViewModel() { }
		private BookingViewModel(int id, int rentalId, DateTime start, int nights)
		{
			Id = id;
			RentalId = rentalId;
			Start = start;
			Nights = nights;
		}
		#endregion Constructors

		#region Properties
		public int Id { get; set; }
		public int RentalId { get; set; }
		public DateTime Start { get; set; }
		public int Nights { get; set; }
		#endregion Properties

		#region Static methods
		public static BookingViewModel Create(int id, int rentalId, DateTime start, int nights)
		{
			BookingViewModel result = new BookingViewModel(id, rentalId, start, nights);
			return result;
		}
		#endregion Static methods
	}
}
