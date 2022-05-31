using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/calendar")]
	[ApiController]
	public class CalendarController : ControllerBase
	{
		private readonly IDictionary<int, RentalViewModel> _rentals;
		private readonly IDictionary<int, BookingViewModel> _bookings;

		public CalendarController(
			IDictionary<int, RentalViewModel> rentals,
			IDictionary<int, BookingViewModel> bookings)
		{
			_rentals = rentals;
			_bookings = bookings;
		}

		[HttpGet]
		public CalendarViewModel Get(int rentalId, DateTime start, int nights)
		{
			bool isNightsValid = nights >= 0;
			if (!isNightsValid)
			{
				string errMsg = "Nigts must be positive";
				throw new ApplicationException(errMsg);
			}
			bool containsKey = _rentals.ContainsKey(rentalId);
			if (!containsKey)
			{
				string errMsg = "Rental not found";
				throw new ApplicationException(errMsg);
			}
			RentalViewModel rental = Helpers.GetRental(rentalId, _rentals);
			List<BookingViewModel> bookingsByRental = Helpers.GetBookingsByRental(rental, _bookings);

			List<CalendarDateViewModel> dates = new List<CalendarDateViewModel>();
			for (var i = 0; i < nights; i++)
			{
				CalendarDateViewModel date = new CalendarDateViewModel
				{
					Date = start.Date.AddDays(i),
					Bookings = new List<CalendarBookingViewModel>(),
					PreparationTimes = new List<PreparationTimeViewModel>()
				};

				foreach (BookingViewModel booking in bookingsByRental)
				{
					if (booking.Start <= date.Date &&
						booking.Start.AddDays(booking.Nights) > date.Date)
					{
						//1 unit per 1 booking. It is set to default in Create();
						CalendarBookingViewModel bookingDTO = CalendarBookingViewModel.Create(booking.Id);
						date.Bookings.Add(bookingDTO);

						//1 unit per 1 booking. It is set to default in Create();
						PreparationTimeViewModel preparationTimeDTO = PreparationTimeViewModel.Create();
						date.PreparationTimes.Add(preparationTimeDTO);
					}
				}

				dates.Add(date);
			}

			CalendarViewModel result = CalendarViewModel.Create(rentalId, dates);
			return result;
		}
	}
}
