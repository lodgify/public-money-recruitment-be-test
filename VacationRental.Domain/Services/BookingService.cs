using VacationRental.Domain.Models;
using VacationRental.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VacationRental.Domain.Services
{
	public class BookingService : IBookingService
	{
		private readonly GIRepository<Booking> bookingRepo;

		/// Injected the repository for the Booking table </param>
		public BookingService(GIRepository<Booking> repository)
		{
			this.bookingRepo = repository;
		}
		public Booking GetById(int bookingId)
		{
			var booking = bookingRepo.Query.Where(x => x.Id == bookingId).FirstOrDefault();
			return booking;
		}

		/// Task that stores a booking in DB.
		public async Task<Booking> Add(Booking bookingToAdd, int rentalUnits, int rentalPrepTime)
		{
            try
            {
                //Get booked units during that date
                var bookings = BookingsByDateRange(bookingToAdd.RentalId, bookingToAdd.Start, bookingToAdd.Nights);
                var bookedUnits = bookings.Select(booking => booking.Unit).Distinct();

                //Check if there are available units
                var bookingUnit = 0;
                var unit = 1;
                while (bookingUnit == 0 && unit <= rentalUnits)
                {
                    if (!bookedUnits.Contains(unit))
                    {
                        bookingUnit = unit;
                    }
                    unit++;
                }

                if (bookingUnit == 0) throw new HttpException(HttpStatusCode.BadRequest, "Not available");

                bookingToAdd.Unit = bookingUnit;

                await this.bookingRepo.Add(bookingToAdd);

                var preparationTime = new Booking()
                {
                    Nights = rentalPrepTime,
                    RentalId = bookingToAdd.RentalId,
                    Start = bookingToAdd.Start.AddDays(bookingToAdd.Nights),
                    Unit = bookingUnit,
                    IsPreparationTime = true
                };

                await this.bookingRepo.Add(preparationTime);

                await this.bookingRepo.Save();
                return bookingToAdd;
            }
            catch (Exception exs)
            {
				throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
			}
		}

		/// This method will filter the bookings in the DB by rental Id and a Date Range
		public IQueryable<Booking> BookingsByDateRange(int rentalId, DateTime startDate, int nights)
		{
            try
            {
                var endDate = startDate.AddDays(nights);
                return this.bookingRepo.Query.Where(booking =>
                    booking.RentalId == rentalId
                    && ((booking.Start <= startDate && booking.Start.AddDays(booking.Nights) > startDate)
                    || (booking.Start < endDate && booking.Start.AddDays(booking.Nights) >= endDate)
                    || (booking.Start > startDate && booking.Start.AddDays(booking.Nights) < endDate)));
            }
            catch (Exception ex)
            {
				throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
			}
		}

		/// This method will filter the bookings by date and rental Id, will retrieve both Booking and PreparationTimes
		public IQueryable<Booking> BookByIdAndDateRange(int rentalId, DateTime date, int nights)
		{
            try
            {
                var endDate = date.AddDays(nights);
                return this.bookingRepo.Query.Where(
                    booking => booking.RentalId == rentalId
                    && ((booking.Start <= date && booking.Start.AddDays(booking.Nights) > date)
                    || (booking.Start < endDate && booking.Start.AddDays(booking.Nights) >= endDate)
                    || (booking.Start > date && booking.Start.AddDays(booking.Nights) < endDate)));
            }
            catch (Exception ex)
            {
				throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
			}
		}

		/// This method will check wether a given unit of a given rental is booked in a given date
		public bool BookedByRentalUnitAndDate(int rentalId, int unit, DateTime date)
		{
            try
            {
                return this.bookingRepo.Query.Any(booking =>
                        booking.RentalId == rentalId
                        && booking.Unit == unit
                        && booking.Start <= date
                        && booking.Start.AddDays(booking.Nights) > date);
            }
            catch (Exception ex)
            {
                throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
            }
		}
		/// This method will get the Preparation Times of a given rental in a given date
		public IQueryable<Booking> PreparationTimesByRentalAndDate(int rentalId, DateTime date)
		{
            try
            {
                return this.bookingRepo.Query.Where(booking =>
                        booking.RentalId == rentalId
                        && (booking.Start > date || booking.Start <= date && booking.Start.AddDays(booking.Nights) > date)
                        && booking.IsPreparationTime);
            }
            catch (Exception ex)
            {
                throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
            }
		}

		/// This method will get the Bokings of a given rental, unit and date range
		public IQueryable<Booking> BookingsByDateRangeAndUnit(int rentalId, int unit, DateTime date, int nights)
		{
            try
            {
                var endDate = date.AddDays(nights);
                return this.bookingRepo.Query.Where(booking => booking.RentalId == rentalId
                    && booking.Unit == unit
                    && ((booking.Start <= date && booking.Start.AddDays(booking.Nights) > date)
                    || (booking.Start < endDate && booking.Start.AddDays(booking.Nights) >= endDate)
                    || (booking.Start > date && booking.Start.AddDays(booking.Nights) < endDate)));
            }
            catch (Exception ex)
            {
                throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
            }
		}

		/// This method will update the preparation time of a booking
		public void UpdatePreparationTimes(IEnumerable<Booking> preparationTimes, int preparationTimeInDays)
		{
            try
            {
                foreach (var preparationTime in preparationTimes)
                {
                    preparationTime.Nights = preparationTimeInDays;
                    bookingRepo.Update(preparationTime);
                }
                bookingRepo.Save();
            }
            catch (Exception ex)
            {
                throw new HttpException(HttpStatusCode.InternalServerError, "Error Occured");
            }
		}
	}
}