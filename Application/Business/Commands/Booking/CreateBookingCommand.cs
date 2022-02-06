using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Booking.Requests;
using Domain.DAL;
using MediatR;

namespace Application.Business.Commands.Booking
{
    public class CreateBookingCommand : IRequest<ResourceIdViewModel>
    {
        public CreateBookingRequest Request { get; }
        
        public CreateBookingCommand(CreateBookingRequest request)
        {
            Request = request;
        }
    }
    
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ResourceIdViewModel>
    {
        private readonly IRepository<Domain.DAL.Models.Booking> _bookingRepository;
        private readonly IRepository<Domain.DAL.Models.Rental> _rentalRepository;

        public CreateBookingCommandHandler(IRepository<Domain.DAL.Models.Booking> bookingRepository, IRepository<Domain.DAL.Models.Rental> rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public Task<ResourceIdViewModel> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var model = command.Request;
            
            var newBooking = new Domain.DAL.Models.Booking
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = model.Unit,
                IsPreparation = false
            };

            var newBookingKey = _bookingRepository.Insert(newBooking);

            _bookingRepository.Insert(new Domain.DAL.Models.Booking
            {
                Nights = _rentalRepository.Find(model.RentalId).PreparationTimeInDays,
                RentalId = model.RentalId,
                Start = newBooking.LastDay,
                Unit = model.Unit,
                IsPreparation = true
            });

            return Task.FromResult(new ResourceIdViewModel()
            {
                Id = newBookingKey
            });
        }
    }
}