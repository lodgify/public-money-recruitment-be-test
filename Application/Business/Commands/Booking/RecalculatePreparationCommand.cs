using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.DAL;
using MediatR;

namespace Application.Business.Commands.Booking
{
    public class RecalculatePreparationCommand : IRequest<Unit>
    {
        public int RentalId { get; }

        public RecalculatePreparationCommand(int rentalId)
        {
            RentalId = rentalId;
        }
    }

    public class RecalculateBookingCommandHandler : IRequestHandler<RecalculatePreparationCommand, Unit>
    {
        private readonly IRepository<Domain.DAL.Models.Booking> _bookingRepository;
        private readonly IRepository<Domain.DAL.Models.Rental> _rentalRepository;
        private readonly ITimeProviderService _timeProviderService;

        public RecalculateBookingCommandHandler(IRepository<Domain.DAL.Models.Booking> bookingRepository,
            IRepository<Domain.DAL.Models.Rental> rentalRepository, ITimeProviderService timeProviderService)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
            _timeProviderService = timeProviderService;
        }

        public Task<Unit> Handle(RecalculatePreparationCommand request, CancellationToken cancellationToken)
        {
            var bookingsToUpdate = _bookingRepository.Query.Values
                .Where(b => b.RentalId == request.RentalId && b.IsPreparation && b.LastDay >= _timeProviderService.Now()).ToList();

            if (!bookingsToUpdate.Any()) return Task.FromResult(Unit.Value);
            
            var rental = _rentalRepository.Find(request.RentalId);

            bookingsToUpdate.ForEach(b =>
            {
                _bookingRepository.Update(new Domain.DAL.Models.Booking()
                {
                    Id = b.Id,
                    Nights = rental.PreparationTimeInDays,
                    Unit = b.Unit,
                    RentalId = b.RentalId,
                    Start = b.Start,
                    IsPreparation = true
                });
            });

            return Task.FromResult(Unit.Value);
        }
    }
}