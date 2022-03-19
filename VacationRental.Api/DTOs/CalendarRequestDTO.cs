using Microsoft.AspNetCore.Mvc;
using System;

namespace VacationRental.WebAPI.DTOs
{
	public class CalendarRequestDTO
	{
		private DateTime _startIgnoreTime;

		[FromQuery]
		public int RentalId { get; set; }

		[FromQuery]
		public DateTime Start
		{
			get => _startIgnoreTime;
			set => _startIgnoreTime = value.Date;
		}

		[FromQuery]
		public int Nights { get; set; }

	}
}
