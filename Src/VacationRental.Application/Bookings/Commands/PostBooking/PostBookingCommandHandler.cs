using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.Services;
using VacationRental.Application.Common.Services.BookingSearchService;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Bookings.Commands.PostBooking
{
    public class PostBookingCommandHandler : IRequestHandler<PostBookingCommand, ResourceIdViewModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingSearchService _service;
        
        public PostBookingCommandHandler(IBookingRepository bookingRepository,
            IRentalRepository rentalRepository, IBookingSearchService service)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
            _service = service;
        }

        public async Task<ResourceIdViewModel> Handle(PostBookingCommand command, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.Get(command.Model.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var bookingsByRental = _bookingRepository.GetByRentalId(rental.Id);

            var bookings = _service.GetBookingsByRangeOfTime(new GetBookingsByRangeOfTimeDTO
            {
                Bookings = bookingsByRental,
                Start = command.Model.Start,
                Nights = command.Model.Nights,
                PreparationTime = rental.PreparationTimeInDays
            });

            if (bookings.Count() >= rental.Units)
                throw new ApplicationException("Not available");
            
            var key = _bookingRepository.Add(new BookingModel
            {
                Nights = command.Model.Nights,
                RentalId = command.Model.RentalId,
                Start = command.Model.Start.Date
            });

            return await Task.FromResult(new ResourceIdViewModel() {Id = key});
        }
    }
}
