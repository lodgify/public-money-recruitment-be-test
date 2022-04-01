using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Bookings.Commands.PostBooking
{
    public class PostBookingCommandHandler : IRequestHandler<PostBookingCommand, ResourceIdViewModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        
        public PostBookingCommandHandler(IBookingRepository bookingRepository,
            IRentalRepository rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<ResourceIdViewModel> Handle(PostBookingCommand command, CancellationToken cancellationToken)
        {
            var newBooking = command.Model;

            var rental = _rentalRepository.Get(newBooking.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var bookingsByRental = _bookingRepository.GetByRentalId(rental.Id);

            var bookings = bookingsByRental.Where(book =>
                book.Start.Date < newBooking.Start.Date.AddDays(newBooking.Nights) &&
                book.Start.Date.AddDays(book.Nights + rental.PreparationTimeInDays) > newBooking.Start.Date);

            if (bookings.Count() >= rental.Units)
                throw new ApplicationException("Not available");
            
            var lastId = _bookingRepository.GetLastId();
            
            var key = _bookingRepository.Save(new BookingModel
            {
                Id = lastId + 1,
                Nights = newBooking.Nights,
                RentalId = newBooking.RentalId,
                Start = newBooking.Start.Date
            });

            return await Task.FromResult(new ResourceIdViewModel() {Id = key});
        }
    }
}