using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Booking.Domain.Interfaces;
using VacationRental.Domain.Interfaces;
using VacationRental.Rental.Entities.Rentals.Events;

namespace VacationRental.Booking.Domain.EventHandlers
{
    public class RentalEventHandler : INotificationHandler<OnRentalUpdatedDomainEvent>
    {
        private readonly ILogger<RentalEventHandler> _logger;
        private IUnitOfWork UnitOfWork { get; }
        public RentalEventHandler(ILogger<RentalEventHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            UnitOfWork = unitOfWork;
        }
        public async Task Handle(OnRentalUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var bookingRepository = UnitOfWork.AsyncRepository<Domain.Booking>();
            var bookings = (await bookingRepository.ListAsync(x => notification.Rental.Id == ((IBooking)x).RentalId)).ToList<IBooking>();
            bookings.ForEach((x) =>
            {
                Booking booking = ((Booking)x).Validate(bookings, notification.Rental);
                bookingRepository.UpdateAsync(booking);
            });
            _logger.LogWarning($"RentalUpdated: {((IBaseEntity)notification.Rental).Id} with ${bookings.Count} Bookings updated");
        }
    }
}
