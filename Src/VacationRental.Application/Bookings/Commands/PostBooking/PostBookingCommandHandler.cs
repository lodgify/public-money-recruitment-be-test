using System;
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
            var model = command.Model;

            var rental = _rentalRepository.Get(model.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in _bookingRepository.GetAll())
                {
                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date &&
                            booking.Start.AddDays(booking.Nights) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) &&
                            booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start &&
                            booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }

                if (count >= rental.Units)
                    throw new ApplicationException("Not available");
            }

            var lastId = _bookingRepository.GetLastId();
            
            var key = _bookingRepository.Save(new BookingModel
            {
                Id = lastId + 1,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            return await Task.FromResult(new ResourceIdViewModel() {Id = key});
        }
    }
}