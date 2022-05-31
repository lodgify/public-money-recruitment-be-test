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
	}
}
