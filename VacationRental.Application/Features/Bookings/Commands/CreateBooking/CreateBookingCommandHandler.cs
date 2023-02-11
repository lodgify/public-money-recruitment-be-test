using System;
using System.Collections.Generic;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Entities.Bookings;
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
            
            if(!BookingManager.isBookingAvailable(bookingsByRental, request.Start, request.Nights, rental.Units, rental.PreparationTimeInDays))
                throw new ConflictException(RentalError.RentalNotAvailable);                            

            var bookingId = _bookingsRepository.Add(Booking.Create(request.RentalId, request.Start, request.Nights, request.Units)).Id;            
            
            return new ResourceId { Id = bookingId };
        }
    }
}
