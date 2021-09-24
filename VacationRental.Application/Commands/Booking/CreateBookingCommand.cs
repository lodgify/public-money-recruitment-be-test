using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Commands.Booking
{
    public class CreateBookingCommand : IRequestHandler<BookingCommandRequest, ResourceIdResponse>
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

        public async Task<ResourceIdResponse> Handle(BookingCommandRequest request, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.Get(new RentalId(request.RentalId));
            var bookings = _bookingRepository.GetByRentalId(rental.Id);

            var newBooking = rental.Book(bookings, new BookingPeriod(request.Start, request.Nights));

            _bookingRepository.Add(newBooking);
            _logger.LogInformation($"Booking for the rental '{rental.Id.Id}' from '{request.Start}' for '{request.Nights}' nights has been created");

            await Task.Delay(1);

            return new ResourceIdResponse{Id = newBooking.Id.Id};
        }
    }
}
