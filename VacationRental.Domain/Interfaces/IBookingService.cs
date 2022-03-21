using VacationRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Domain.Interfaces
{
	public interface IBookingService
	{
		public Booking GetById(int bookingId);

		public Task<Booking> Add(Booking bookingToAdd, int rentalUnits, int rentalPrepTime);

		public IQueryable<Booking> BookByIdAndDateRange(int rentalId, DateTime date, int nights);

		public IQueryable<Booking> BookingsByDateRange(int rentalId, DateTime date, int nights);

		public bool BookedByRentalUnitAndDate(int rentalId, int unit, DateTime date);

		public void UpdatePreparationTimes(IEnumerable<Booking> preparationTimes, int preparationTimeInDays);

		public IQueryable<Booking> PreparationTimesByRentalAndDate(int rentaId, DateTime date);

		public IQueryable<Booking> BookingsByDateRangeAndUnit(int rentalId, int unit, DateTime date, int nights);
	}
}
