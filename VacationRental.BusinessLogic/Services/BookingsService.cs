using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using VacationRental.BusinessLogic.Services.Interfaces;
using VacationRental.BusinessObjects;
using VacationRental.Repository.Entities;
using VacationRental.Repository.Repositories.Interfaces;

namespace VacationRental.BusinessLogic.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBooking> _createBookingValidator;
        private readonly IRentalsService _rentalsService;

        public BookingsService(IBookingsRepository bookingsRepository,
            IMapper mapper,
            IValidator<CreateBooking> createBookingValidator,
            IRentalsService rentalsService)
        {
            _bookingsRepository = bookingsRepository;
            _mapper = mapper;
            _createBookingValidator = createBookingValidator;
            _rentalsService = rentalsService;
        }

        public Booking GetBooking(int bookingId)
        {
            var bookingEntity = _bookingsRepository.GetBookingEntity(bookingId);
            if (bookingEntity == null)
            {
                throw new ArgumentException($"Booking with id {bookingId} does not exist.");
            }

            var booking = _mapper.Map<Booking>(bookingEntity);

            return booking;
        }

        public List<Booking> GetBookings()
        {
            var bookingEntities = _bookingsRepository.GetBookingEntities();
            var bookings = _mapper.Map<List<Booking>>(bookingEntities);

            return bookings;
        }

        public int CreateBooking(CreateBooking createBooking)
        {
            _createBookingValidator.ValidateAndThrow(createBooking);
            var rental = _rentalsService.GetRental(createBooking.RentalId);
            if (rental == null)
            {
                throw new ArgumentException($"Rental with id {createBooking.RentalId} not found.");
            }

            var bookings = _bookingsRepository.GetBookingEntities();
            for (var i = 0; i < createBooking.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookings)
                {
                    if (booking.RentalId == createBooking.RentalId
                        && (booking.Start <= createBooking.Start.Date && booking.Start.AddDays(booking.Nights) > createBooking.Start.Date)
                        || (booking.Start < createBooking.Start.AddDays(createBooking.Nights) && booking.Start.AddDays(booking.Nights) >= createBooking.Start.AddDays(createBooking.Nights))
                        || (booking.Start > createBooking.Start && booking.Start.AddDays(booking.Nights) < createBooking.Start.AddDays(createBooking.Nights)))
                    {
                        count++;
                    }
                }

                if (count >= rental.Units)
                {
                    throw new ApplicationException("Not available.");
                }
            }

            var bookingEntity = _mapper.Map<BookingEntity>(createBooking);
            var bookingId = _bookingsRepository.CreateBookingEntity(bookingEntity);

            return bookingId;
        }
    }
}
