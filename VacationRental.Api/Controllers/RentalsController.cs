using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/rentals")]
	[ApiController]
	public class RentalsController : ControllerBase
	{
		private readonly IDictionary<int, RentalViewModel> _rentals;
		private readonly IDictionary<int, BookingViewModel> _bookings;

		public RentalsController(
			IDictionary<int, RentalViewModel> rentals,
			IDictionary<int, BookingViewModel> bookings)
		{
			_rentals = rentals;
			_bookings = bookings;
		}

		[HttpGet]
		[Route("{rentalId:int}")]
		public RentalViewModel Get(int rentalId)
		{
			bool containsKey = _rentals.ContainsKey(rentalId);
			if (!containsKey)
			{
				string errMsg = "Rental not found";
				throw new ApplicationException(errMsg);
			}

			RentalViewModel result = _rentals[rentalId];
			return result;
		}

		[HttpPost]
		public ResourceIdViewModel Post(RentalBindingModel model)
		{
			int newId = _rentals.Keys.Count + 1;
			ResourceIdViewModel result = ResourceIdViewModel.Create(newId);

			RentalViewModel rental = RentalViewModel.Create(result.Id, model.Units, model.PreparationTimeInDays);
			_rentals.Add(result.Id, rental);

			return result;
		}

		[HttpPut]
		[Route("{rentalId:int}")]
		public RentalViewModel Put(int rentalId, RentalBindingModel rental)
		{
			bool containsKey = _rentals.ContainsKey(rentalId);
			if (!containsKey)
			{
				string errMsg = "Rental not found";
				throw new ApplicationException(errMsg);
			}

			List<BookingViewModel> bookingsTemp = new List<BookingViewModel>();
			List<BookingViewModel> bookingsWithRentalTemp = _bookings.Values.Where(r => r.RentalId == rentalId).ToList();

			RentalViewModel tempRental = RentalViewModel.Create(rental.Units, rental.PreparationTimeInDays);
			//Validate if changes cause over booking. Throw a new API exception if it does.
			foreach (BookingViewModel bookingWithRental in bookingsWithRentalTemp)
			{
				bool isRentalAvailable = Helpers.IsRentalAvailable(
					tempRental,
					bookingsTemp,
					bookingWithRental.Start,
					bookingWithRental.Nights);

				if (!isRentalAvailable)
				{
					string errMsg = "ERROR: Over booking detected";
					throw new ApplicationException(errMsg);
				}

				int newBookingId = bookingsTemp.Count + 1;
				BookingViewModel newBookingWithRental = BookingViewModel.Create(
					newBookingId,
					rentalId,
					bookingWithRental.Start,
					bookingWithRental.Nights);

				bookingsTemp.Add(newBookingWithRental);
			}

			RentalViewModel rentalToBeUpdated = _rentals[rentalId];
			rentalToBeUpdated.PreparationTimeInDays = rental.PreparationTimeInDays;
			rentalToBeUpdated.Units = rental.Units;

			return rentalToBeUpdated;
		}
	}
}
