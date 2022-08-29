using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VacationRental.Core;
using VacationRental.Core.Data;
using VacationRental.Core.Domain.Bookings;
using VacationRental.Core.Domain.Bookings.Spec;
using VacationRental.Core.Domain.Rentals;
using VacationRental.Services.Exceptions;
using VacationRental.Services.Models.Booking;

namespace VacationRental.Services.Bookings
{
    public class BookingsService : IBookingsService
    {
        private readonly IRepository<BookingEntity, int> _bookingRepository;
        private readonly IRepository<RentalEntity, int> _rentalRepository;
        private readonly ILogger<BookingsService> _logger;
        private readonly IMapper _mapper;

        public BookingsService(
            IRepository<BookingEntity, int> bookingRepository,
            IRepository<RentalEntity, int> rentalRepository,
            ILogger<BookingsService> logger,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<BookingDto> GetBookings()
        {
            var query = _bookingRepository
                .Table;

            return _mapper.ProjectTo<BookingDto>(query);
        }

        public BookingDto GetBookingById(int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);
            if (booking == null)
            {
                throw new BookingNotFoundException();
            }

            return _mapper.Map<BookingDto>(booking);
        }

        public BookingDto AddBooking(CreateBookingRequest request)
        {
            Guard.NotNull(request, nameof(request));
            
            var rental = _rentalRepository.GetById(request.RentalId);
            if (rental == null)
            {
                throw new RentalNotFoundException("Rental not found");
            }

            var hasRentalId = new FilterByRentalId(request.RentalId);

            var bookings = _bookingRepository
                .Table
                .Where(hasRentalId.ToExpression()) // TODO: Implement implicit operator 
                .ToList();

            var count = bookings
                .Count(booking => (booking.Start <= request.Start && booking.End > request.Start)
                                  || (booking.Start < request.End && booking.End >= request.End)
                                  || (booking.Start > request.Start && booking.End < request.End));

            if (count >= rental.Units)
            {
                throw new InvalidBookingException("Not available");
            }

            var entity = _mapper.Map<BookingEntity>(request);
            _bookingRepository.Insert(entity);
            
            var dto = _mapper.Map<BookingDto>(entity);

            return dto;
        }

        public BookingDto UpdateBooking(int bookingId, CreateBookingRequest request)
        {
            Guard.NotNull(request, nameof(request));

            var rental = _rentalRepository.GetById(request.RentalId);
            if (rental == null)
            {
                throw new RentalNotFoundException("Rental not found");
            }

            var count = _bookingRepository.Table
                .Count(booking => booking.RentalId == request.RentalId
                                  && (booking.Start <= request.Start.Date && booking.Start.AddDays(booking.Nights) > request.Start.Date)
                                  || (booking.Start < request.Start.AddDays(request.Nights) && booking.Start.AddDays(booking.Nights) >= request.Start.AddDays(request.Nights))
                                  || (booking.Start > request.Start && booking.Start.AddDays(booking.Nights) < request.Start.AddDays(request.Nights)));

            if (count >= rental.Units)
            {
                throw new InvalidBookingException("Not available");
            }

            var entity = _bookingRepository.GetById(bookingId);
            if (entity == null)
            {
                throw new BookingNotFoundException("Booking not found");
            }

            entity.RentalId = request.RentalId;
            entity.Start = request.Start;
            entity.Nights = request.Nights;

            _bookingRepository.Update(entity);

            var dto = _mapper.Map<BookingDto>(entity);

            return dto;
        }

        public bool DeleteBooking(int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);
            if (booking == null)
            {
                throw new RentalNotFoundException("Not found");
            }

            try
            {
                _bookingRepository.Delete(booking);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred deleting bookingId: {bookingId}.");

                return false;
            }

            return true;
        }
    }
}
