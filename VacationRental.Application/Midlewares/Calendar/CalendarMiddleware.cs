using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Application.ViewModels;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Application.Midlewares.Calendar
{
	public class CalendarMiddleware : ICalendarMiddleware
	{
		private readonly IRentalRepository _rentalRepository;
		private readonly IBookingRepository _bookingRepository;
		private readonly ICalendarRepository _calendarRepository;
		private readonly IPreparationTimeRepository _preparationTimeRepository;

		public CalendarMiddleware(IRentalRepository rentalRepository,
			IBookingRepository bookingRepository,
			ICalendarRepository calendarRepository,
			IPreparationTimeRepository preparationTimeRepository)
		{
			this._rentalRepository = rentalRepository;
			this._bookingRepository = bookingRepository;
			this._calendarRepository = calendarRepository;
			this._preparationTimeRepository = preparationTimeRepository;
		}

		private async Task<CalendarViewModel> ReturnCalendarsWithoutBookingAndPrepTime(int rentalId, int nights, DateTime startDate)
		{
			CalendarViewModel calendars = new CalendarViewModel
			{
				RentalId = rentalId,
				Dates = new List<CalendarDateViewModel>()
			};

			CalendarDateViewModel calendarDate;

			var bookingsInRental = await this._bookingRepository.GetAllBookingsByRentalId(rentalId);
			var preparationTimesInRental = await this._preparationTimeRepository.GetAllPreparationTimeInRental(rentalId);

			if (bookingsInRental.Count == 0 && preparationTimesInRental.Count == 0)
			{
				for (var i = 0; i < nights; i++)
				{
					calendarDate = new CalendarDateViewModel
					{
						Date = startDate.Date.AddDays(i),
						Bookings = new List<CalendarBookingViewModel>()
					};

					calendars.Dates.Add(calendarDate);
				}
			}

			return calendars;
		}

		private async Task<CalendarViewModel> ReturnCalendarsWithBooking(Domain.Rentals.Rental rental, int nights, DateTime startDate)
		{
			CalendarViewModel calendars = new CalendarViewModel
			{
				RentalId = rental.Id,
				Dates = new List<CalendarDateViewModel>()
			};

			CalendarDateViewModel calendarDate;

			var bookingsInRental = await this._bookingRepository.GetAllBookingsByRentalId(rental.Id);

			if (bookingsInRental.Count > 0)
			{
				for (var i = 0; i < nights; i++)
				{
					calendarDate = new CalendarDateViewModel
					{
						Date = startDate.Date.AddDays(i),
						Bookings = new List<CalendarBookingViewModel>()
					};

					foreach (var booking in bookingsInRental)
					{
						if (booking.CalendarDate.StartDate <= calendarDate.Date && booking.CalendarDate.StartDate.AddDays(booking.Nights) > calendarDate.Date)
						{
							if (booking.Unity > rental.Units)
							{
								throw new ApplicationException("Booking unity cannot be higher than rental unities");
							}

							calendarDate.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
						}

						calendars.Dates.Add(calendarDate);
					}
				}
			}

			return calendars;
		}

		public async Task<CalendarViewModel> GetAvailableCalendar(CalendarInputViewModel input)
		{
			CalendarViewModel bookingAvailables = null;

			if (input.Nights < 0)
			{
				throw new ApplicationException("Nights must be positive");
			}

			var rental = await this._rentalRepository.GetById(input.RentalId);

			if (rental == null)
			{
				throw new ApplicationException("Rental not found");
			}

			//this scenario is where we don't have any bookings and preparation times for this rental
			if (input.Bookings.Count == 0 && input.PreparationTimes.Count == 0)
			{
				bookingAvailables = await this.ReturnCalendarsWithoutBookingAndPrepTime(input.RentalId, input.Nights, input.DateStart);
			}

			if (input.Bookings.Count > 0)
			{
				bookingAvailables = await ReturnCalendarsWithBooking(rental, input.Nights, input.DateStart);
			}

			return bookingAvailables;
		}


	}
}
