using System;

namespace VacationRental.Application.Dtos
{
	public class BookingInputDto
	{
		public BookingInputDto(int rentalId, DateTime start, int nights)
		{
			this.RentalId = rentalId;
			this.Start = start;
			this.Nights = nights;
		}

		public int RentalId { get; set; }

		public DateTime Start { get; set; }

		public int Nights { get; set; }
	}
}
