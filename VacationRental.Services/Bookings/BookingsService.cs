using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using VacationRental.Core;
using VacationRental.Core.Data;
using VacationRental.Core.Domain.Bookings;
using VacationRental.Core.Domain.Rentals;
using VacationRental.Services.Exceptions;
using VacationRental.Services.Models;
using VacationRental.Services.Models.Booking;
using VacationRental.Services.Models.Calendar;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Services.Bookings
{
    public class BookingsService : IBookingsService
    {
        private readonly IRepository<BookingEntity, int> _bookingRepository;
        private readonly IRepository<RentalEntity, int> _rentalRepository;
        private readonly IConfigurationProvider _mappingConfiguration;

        public BookingsService(
            IRepository<BookingEntity, int> bookingRepository,
            IRepository<RentalEntity, int> rentalRepository,
            IConfigurationProvider mappingConfiguration)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
            _mappingConfiguration = mappingConfiguration;
        }

        public BookingResponse Get(int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);

            return _mappingConfiguration.CreateMapper().Map<BookingResponse>(booking);
        }

        public BookingEntity Book(CreateBookingRequest request)
        {
            Guard.NotNull(request, nameof(request));
            if (request.Nights <= 0)
            {
                throw new ApplicationException("Nights must be positive");
            }

            var rental = _rentalRepository.GetById(request.RentalId);
            if (rental == null)
            {
                throw new RentalNotFoundException("Rental not found");
            }

            var bookings = _bookingRepository
                .GetAll()
                .ToList();

            for (var i = 0; i < request.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookings)
                {
                    if (booking.RentalId == request.RentalId
                        && (booking.Start <= request.Start.Date && booking.Start.AddDays(booking.Nights) > request.Start.Date)
                        || (booking.Start < request.Start.AddDays(request.Nights) && booking.Start.AddDays(booking.Nights) >= request.Start.AddDays(request.Nights))
                        || (booking.Start > request.Start && booking.Start.AddDays(booking.Nights) < request.Start.AddDays(request.Nights)))
                    {
                        count++;
                    }
                }

                if (count >= rental.Units)
                {
                    throw new ApplicationException("Not available");
                }
            }

            var entity = new BookingEntity
            {
                Nights = request.Nights,
                RentalId = request.RentalId,
                Start = request.Start.Date
            };
            _bookingRepository.Insert(entity);
            
            return entity;
        }

        public IEnumerable<OverlappedBookingViewModel> GetOverlappings(RentalViewModel rental)
        {
            throw new NotImplementedException();
        }
    }
}
