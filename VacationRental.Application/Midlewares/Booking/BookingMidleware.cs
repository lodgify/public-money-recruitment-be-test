using System;
using System.Threading.Tasks;
using VacationRental.Application.Dtos;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.Calendars;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Application.Midlewares.Booking
{
	public class BookingMidleware : IBookingMiddleware
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingMidleware(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            this._rentalRepository = rentalRepository;
            this._bookingRepository = bookingRepository;
        }

		public async Task<BookingViewModelOutput> GetBookingById(int id)
		{
			var response = await this._bookingRepository.GetBooking(id);

			if (response == null) 
			{
				throw new ApplicationException("Booking not found");
			}

			var booking = new BookingViewModelOutput(response.Id, response.RentalId, response.CalendarDate.StartDate, response.Nights);
			return booking;
		}


		public async Task<int> CreateBooking(BookingInputDto input)
        {
			if (input.Nights <= 0) 
			{
				throw new ApplicationException("Nigts must be positive");
			}

			if (input.RentalId == default) 
			{
				throw new ApplicationException("It has to be a rental");
			}

			var rental = await this._rentalRepository.GetById(input.RentalId);

			if (rental == null) 
			{
				throw new ApplicationException("Rental not found");
			}

			CalendarDate calendarDate;
			Domain.Bookings.Booking bookingEntity;

			if (rental.Bookings == null) 
			{
				if (input.Nights >= rental.Units) 
				{
					throw new ApplicationException("Not available");
				}

				calendarDate = new CalendarDate(input.Start, input.Start.AddDays(input.Nights));
				bookingEntity = new Domain.Bookings.Booking(calendarDate, rental, input.Nights, input.Nights);
				var booking = await this._bookingRepository.CreateBooking(bookingEntity);
				return booking.Id;
			}

			int countNights = 0;

			for (var i = 0; i < input.Nights; i++)
			{
				foreach (var booking in rental?.Bookings)
				{
					if (CheckDateAvailability(input, booking))
					{
						countNights++;
					}
				}

				if (countNights >= rental.Units)
					throw new ApplicationException("Not available");
			}

			calendarDate = new CalendarDate(input.Start, input.Start.AddDays(input.Nights));
			bookingEntity = new Domain.Bookings.Booking(calendarDate, rental, input.Nights, countNights);
			rental.Bookings.Add(bookingEntity);
			var bookingCreated = await this._bookingRepository.CreateBooking(bookingEntity);
			return bookingCreated.Id;
		}

		

		private bool CheckDateAvailability(BookingInputDto input, Domain.Bookings.Booking booking) 
		{
			return (booking.CalendarDate.StartDate <= booking.CalendarDate.StartDate.Date && booking.CalendarDate.StartDate.AddDays(booking.Nights) > input.Start.Date)
						|| (booking.CalendarDate.StartDate < input.Start.AddDays(input.Nights) && booking.CalendarDate.StartDate.AddDays(booking.Nights) >= input.Start.AddDays(input.Nights))
						|| (booking.CalendarDate.StartDate > input.Start && booking.CalendarDate.StartDate.AddDays(booking.Nights) < input.Start.AddDays(input.Nights));
		}
    }
}
