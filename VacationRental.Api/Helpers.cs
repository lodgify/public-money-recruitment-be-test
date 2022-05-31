using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
	public static class Helpers
	{
		public static RentalViewModel GetRental(
			int rentalId,
			 IDictionary<int, RentalViewModel> rentals)
		{
			RentalViewModel result = null;
			rentals.TryGetValue(rentalId, out result);
			return result;
		}

		public static List<BookingViewModel> GetBookingsByRental(
			RentalViewModel rental,
			IDictionary<int, BookingViewModel> bookings)
		{
			List<BookingViewModel> result = bookings.Values.Where(
					r =>
					r.RentalId == rental.Id)
					.ToList();

			return result;
		}

		public static bool IsRentalAvailable(
			RentalViewModel rental,
			List<BookingViewModel> bookingsByRental,
			DateTime requestStartTime,
			int requestNights)
		{
			bool result = false;
			for (var i = 0; i < requestNights; i++)
			{
				int additionalUnitRequired = 0;
				foreach (BookingViewModel booking in bookingsByRental)
				{
					DateTime bookingStartDate = booking.Start;
					DateTime bookingEndDate = bookingStartDate.AddDays(booking.Nights);
					DateTime bookingAvailableAfter = bookingEndDate.AddDays(rental.PreparationTimeInDays);

					DateTime requestStartDate = requestStartTime.Date;
					DateTime requestEndDate = requestStartTime.AddDays(requestNights);
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
