using VacationRental.Domain.Models;
using VacationRental.Domain.Models.Interfaces;
using VacationRental.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace VacationRental.Domain.Services
{
	public class CalendarService : ICalendarService
	{

		private readonly IBookingService bookingService;
		private readonly IRentalService rentalService;

		public CalendarService(IBookingService bookingService,
							   IRentalService rentalService)
		{
			this.bookingService = bookingService;
			this.rentalService = rentalService;
		}

		public Calendar Get(CalendarRequest calendarRequest)
		{
			if (rentalService.GetById(calendarRequest.RentalId) == null)
				throw new HttpException(HttpStatusCode.NotFound, "Rental not found");

			var result = new Calendar
			{
				RentalId = calendarRequest.RentalId,
				Dates = new List<CalendarDate>()
			};

			var bookings = bookingService.BookByIdAndDateRange(calendarRequest.RentalId, calendarRequest.Start.Date, calendarRequest.Nights).ToList();
			for (var i = 0; i < calendarRequest.Nights; i++)
			{
				var calendarDate = new CalendarDate
				{
					Date = calendarRequest.Start.Date.AddDays(i),
					Bookings = new List<IBookingPeriod>(),
					PreparationTimes = new List<IBookingPeriod>()
				};

				foreach (var booking in bookings)
				{
					if (booking.IsPreparationTime)
					{
						calendarDate.PreparationTimes.Add(new CalendarBooking { Id = booking.Id, Unit = booking.Unit });
					}
					else
					{
						calendarDate.Bookings.Add(new CalendarBooking { Id = booking.Id, Unit = booking.Unit });
					}
				}
				result.Dates.Add(calendarDate);
			}

			return result;
		}
	}
}