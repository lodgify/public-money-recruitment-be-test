using VacationRental.Domain.Models;
using VacationRental.Domain.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VacationRental.Domain.Services
{
	public class RentalBookingService : IRentalBookingService
	{
		private readonly IBookingService bookingService;
		private readonly IRentalService rentalService;

		/// Rental-Booking Service constructor.
		public RentalBookingService(IBookingService bookingService,
									IRentalService rentalService)
		{
			this.bookingService = bookingService;
			this.rentalService = rentalService;
		}

		public async Task<Booking> AddBooking(Booking bookingToAdd)
		{
			var rental = rentalService.GetById(bookingToAdd.RentalId);
			if (rental == null) throw new HttpException(HttpStatusCode.NotFound, "Rental not found");
			return await bookingService.Add(bookingToAdd, rental.Units, rental.PreparationTimeInDays);
		}

		public Rental UpdateRental(Rental newRental)
		{
			var rentalToUpdate = rentalService.GetById(newRental.Id);

			if (rentalToUpdate == null) throw new HttpException(HttpStatusCode.NotFound, "Rental not found");

			if (newRental.Units < rentalToUpdate.Units)
			{
				//Loop through all the units that would be removed
				for (int unit = newRental.Units + 1; unit <= rentalToUpdate.Units; unit++)
				{
					if (bookingService.BookedByRentalUnitAndDate(rentalToUpdate.Id, unit, DateTime.Now.Date))
					{
						throw new HttpException(HttpStatusCode.Conflict, $"Unit {unit} cannot be removed because it is already booked.");
					}
				}
			}

			var preparationTimes = bookingService.PreparationTimesByRentalAndDate(newRental.Id, DateTime.Now.Date).ToList();

			foreach (var preparationTime in preparationTimes)
			{
				var overlappedBookings = bookingService.BookingsByDateRangeAndUnit(newRental.Id, preparationTime.Unit, preparationTime.Start, newRental.PreparationTimeInDays);

				overlappedBookings = overlappedBookings.Where(booking => booking.Id != preparationTime.Id);

				if (overlappedBookings.Count() > 0)
				{
					throw new HttpException(HttpStatusCode.Conflict, $"There would be an overlapping between preparation time {preparationTime.Id} and booking {string.Join(", ", overlappedBookings.Select(b => b.Id))}");
				}
			}
			if (newRental.PreparationTimeInDays != rentalToUpdate.PreparationTimeInDays)
			{
				bookingService.UpdatePreparationTimes(preparationTimes, newRental.PreparationTimeInDays);
			}

			rentalService.Update(newRental);
			return newRental;
		}
	}
}
