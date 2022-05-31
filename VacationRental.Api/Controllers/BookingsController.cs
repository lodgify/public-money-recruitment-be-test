using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/bookings")]
	[ApiController]
	public class BookingsController : ControllerBase
	{
		private readonly IDictionary<int, RentalViewModel> _rentals;
		private readonly IDictionary<int, BookingViewModel> _bookings;

		public BookingsController(
			IDictionary<int, RentalViewModel> rentals,
			IDictionary<int, BookingViewModel> bookings)
		{
			_rentals = rentals;
			_bookings = bookings;
		}

		[HttpGet]
		[Route("{bookingId:int}")]
		public BookingViewModel Get(int bookingId)
		{
			bool containsKey = _bookings.ContainsKey(bookingId);
			if (!containsKey)
			{
				string errMsg = "Booking not found";
				throw new ApplicationException(errMsg);
			}

			BookingViewModel result = _bookings[bookingId];
			return result;
		}

		[HttpPost]
		public ResourceIdViewModel Post(BookingBindingModel model)
		{
			bool isNightsValid = model.Nights > 0;
			if (!isNightsValid)
			{
				string errMsg = "Nigts must be positive";
				throw new ApplicationException(errMsg);

			}
			bool containsKey = _rentals.ContainsKey(model.RentalId);
			if (!containsKey)
			{
				string errMsg = "Rental not found";
				throw new ApplicationException(errMsg);
			}

			RentalViewModel rental = Helpers.GetRental(model.RentalId, _rentals);
			bool isRentalAvailable = IsRentalAvailable(rental, model);
			if (!isRentalAvailable)
			{
				string errMsg = "Not available";
				throw new ApplicationException(errMsg);
			}

			int newBookingId = _bookings.Keys.Count + 1;
			ResourceIdViewModel key = ResourceIdViewModel.Create(newBookingId);
			BookingViewModel newBooking = BookingViewModel.Create(key.Id, model.RentalId, model.Start.Date, model.Nights);
			_bookings.Add(key.Id, newBooking);

			return key;
		}

		private bool IsRentalAvailable(RentalViewModel rental, BookingBindingModel request)
		{
			bool result = false;
			List<BookingViewModel> bookingsByRental = Helpers.GetBookingsByRental(rental, _bookings);
			for (var i = 0; i < request.Nights; i++)
			{
				int additionalUnitRequired = 0;
				foreach (BookingViewModel booking in bookingsByRental)
				{
					DateTime bookingStartDate = booking.Start;
					DateTime bookingEndDate = bookingStartDate.AddDays(booking.Nights);
					DateTime bookingAvailableAfter = bookingEndDate.AddDays(rental.PreparationTimeInDays);

					DateTime requestStartTime = request.Start;
					DateTime requestStartDate = requestStartTime.Date;

					DateTime requestEndDate = requestStartTime.AddDays(request.Nights);
					DateTime requestCompleteDate = requestEndDate.AddDays(rental.PreparationTimeInDays);

					bool isAdditionalUnitRequired =
						(bookingStartDate <= requestStartDate && bookingAvailableAfter > requestStartDate) ||
						(bookingStartDate < requestCompleteDate && bookingAvailableAfter >= requestCompleteDate) ||
						(bookingStartDate > requestStartTime && bookingAvailableAfter < requestCompleteDate);

					if (isAdditionalUnitRequired)
					{
						additionalUnitRequired++;
					}
				}

				if (additionalUnitRequired >= rental.Units)
				{
					return result;
				}
			}

			result = true;
			return result;
		}
	}
}
