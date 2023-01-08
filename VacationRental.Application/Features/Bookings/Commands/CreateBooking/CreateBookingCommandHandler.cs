using System;
using System.Collections.Generic;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Errors;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;
using ValidationException = VacationRental.Application.Exceptions.ValidationException;

namespace VacationRental.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : ICommandHandler<CreateBookingCommand, ResourceId>
    {
        private readonly IBookingRepository _bookingsRepository;
        private readonly IRepository<Rental> _rentalsRepository;        

        public CreateBookingCommandHandler(IBookingRepository bookingsRepository, IRepository<Rental> rentalRepository)
        {
            _bookingsRepository = bookingsRepository;
            _rentalsRepository = rentalRepository;            
        }

        public ResourceId Handle(CreateBookingCommand request)
        {
            var rental = _rentalsRepository.GetById(request.RentalId);
            if (rental == null)
                throw new NotFoundException(RentalError.RentalNotFound);
            
            var bookingsByRental = _bookingsRepository.GetBookingByRentalId(request.RentalId);
            var numberOfBookings = 0;
            
            for (var i = 0; i < request.Nights; i++)
            {
                numberOfBookings = CalculateNumberOfBookings(bookingsByRental, request.Start, request.Nights);
            }
            
            if (numberOfBookings >= rental.Units)
                throw new ConflictException(RentalError.RentalNotAvailable);

            var bookingId = _bookingsRepository.Add(Booking.Create(request.RentalId, request.Start, request.Nights, request.Units)).Id;            
            
            return new ResourceId { Id = bookingId };
        }
        

        private int CalculateNumberOfBookings(IReadOnlyList<Booking> bookings, DateTime start, int nights)
        {
            var numberOfBookings = 0;

            foreach (var booking in bookings)
            {
                if ((booking.Start <= start.Date && booking.Start.AddDays(booking.Nights) > start.Date)
                || (booking.Start < start.AddDays(nights) && booking.Start.AddDays(booking.Nights) >= start.AddDays(nights))
                    || (booking.Start > start && booking.Start.AddDays(booking.Nights) < start.AddDays(nights)))
                {
                    numberOfBookings++;
                }
            }

            return numberOfBookings;
        }
    }
}
