using Flunt.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Notifications;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Persistance.Interfaces;

namespace VacationRental.Application.Handlers.Bookings
{
    public class GetBookingHandler : IRequestHandler<GetBookingRequest, EntityResult<BookingViewModel>>
    {
        private readonly IRepository<BookingEntity> _bookingRepository;

        public GetBookingHandler(IRepository<BookingEntity> bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        async Task<EntityResult<BookingViewModel>> IRequestHandler<GetBookingRequest, EntityResult<BookingViewModel>>.Handle(GetBookingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entityResult = new EntityResult<BookingViewModel>(request.Notifications, null);

                if (entityResult.Valid)
                {
                    var result = _bookingRepository.GetById(request.Id);

                    if (result == null)
                    {
                        return new EntityResult<BookingViewModel>(request.Notifications, null);
                    }
                    if (result != null)
                    {
                        var bookingViewModel = new BookingViewModel()
                        {
                            Id = result.Id,
                            RentalId = result.RentalId,
                            Nights = result.Nights,
                            Start = result.Start
                        };

                        entityResult = new EntityResult<BookingViewModel>(request.Notifications, bookingViewModel);
                    }
                }

                return entityResult;
            }
            catch (Exception ex)
            {
                request.AddNotification(new Notification("GetBookingHandler-Handler", $"Exception - [{ex.Message}]"));
                return new EntityResult<BookingViewModel>(request.Notifications, null) { Error = ErrorCode.InternalServerError };
            }
        }
    }
}
