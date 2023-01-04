using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Contracts.Mediatr;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Models.Bookings;
using VacationRental.Domain.Models.Rentals;

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

        public Task<ResourceId> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var rental = _rentalsRepository.GetById(request.RentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");
            
            var bookingsByRental = _bookingsRepository.GetBookingByRentalId(request.RentalId);
            var numberOfBookings = 0;
            
            for (var i = 0; i < request.Nights; i++)
            {
                numberOfBookings = CalculateNumberOfBookings(bookingsByRental, request.Start, request.Nights);
            }
            
            if (numberOfBookings >= rental.Units)
                throw new ApplicationException("Not available");

            var bookingId = _bookingsRepository.Add(Booking.Create(request.RentalId, request.Start, request.Nights, request.Units)).Id;
            var resourceId = new ResourceId { Id= bookingId };
            return Task.FromResult(resourceId);
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
