using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Commands.Booking
{
    public class CreateBookingCommand : IRequestHandler<BookingRequest, ResourceIdViewModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<CreateBookingCommand> _logger;

        public CreateBookingCommand(IRentalRepository rentalRepository, IBookingRepository bookingRepository, ILogger<CreateBookingCommand> logger)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResourceIdViewModel> Handle(BookingRequest request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.Get(new RentalId(request.RentalId));
            var bookings = await _bookingRepository.GetByRentalId(rental.Id);

            var newBooking = rental.Book(bookings, new BookingPeriod(request.Start, request.Nights));

            newBooking = await _bookingRepository.Add(newBooking); // booking with a generated identifier.
            _logger.LogInformation($"Booking for the rental '{rental.Id}' from '{request.Start}' for '{request.Nights}' nights has been created");

            return new ResourceIdViewModel{Id = (int) newBooking.Id};
        }
    }
}
