using Flunt.Notifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Notifications;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Persistance.Interfaces;

namespace VacationRental.Application.Handlers.Bookings
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingRequest,EntityResult<ResourceIdViewModel>>
    {
        private readonly IRepository<RentalEntity> _rentalRepository;
        private readonly IRepository<BookingEntity> _bookingRepository;

        public CreateBookingHandler(IRepository<RentalEntity> repository, IRepository<BookingEntity> bookingRepository)
        {
            _rentalRepository = repository;
            _bookingRepository = bookingRepository;
        }

        public async Task<EntityResult<ResourceIdViewModel>> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entityResult = new EntityResult<ResourceIdViewModel>(request.Notifications, null);

                if (request.Valid)
                {
                    RentalEntity rental = _rentalRepository.GetById(request.RentalId);

                    if (rental == null)
                    {
                        request.AddNotification(new Notification("CreateBookingHandler-Handler", $"Exception - [invalid rental.]"));
                        return new EntityResult<ResourceIdViewModel>(request.Notifications, null) { Error = ErrorCode.NotFound };
                    }

                    int id = _bookingRepository.GetAll().Select(p => p.Id).DefaultIfEmpty(0).Max() + 1;

                    var booking = new BookingEntity()
                    {
                        Id = id,
                        Nights = request.Nights,
                        Start = request.StartDate,
                        RentalId = request.RentalId
                    };

                    var bookings = _bookingRepository.GetAll();

                    for (var i = 0; i < request.Nights; i++)
                    {
                        var count = 0;
                        foreach (var book in bookings)
                        {
                            var blockedNights = book.Nights + rental.PreparationTimeInDays;

                            if (book.RentalId == request.RentalId
                                && (book.Start <= request.StartDate.Date && book.Start.AddDays(blockedNights) > request.StartDate.Date)
                                || (book.Start < request.StartDate.AddDays(request.Nights) && book.Start.AddDays(blockedNights) >= request.StartDate.AddDays(request.Nights))
                                || (book.Start > request.StartDate && book.Start.AddDays(blockedNights) < request.StartDate.AddDays(request.Nights)))
                            {
                                count++;
                            }
                        }

                        booking.Unit = count + 1;

                        if (count >= rental.Units)
                        {
                            request.AddNotification(new Notification("CreateBookingHandler-Handler", $"Exception - [Date no available for booking.]"));
                            return new EntityResult<ResourceIdViewModel>(request.Notifications, null) { Error = ErrorCode.NotFound };
                        }
                    }

                    await _bookingRepository.InsertAsync(booking);

                    var resourceIdViewModel = new ResourceIdViewModel() { Id = id };
                    entityResult = new EntityResult<ResourceIdViewModel>(request.Notifications, resourceIdViewModel);
                }

                return entityResult;
            }
            catch (Exception ex)
            {
                request.AddNotification(new Notification("CreateBookingHandler-Handler", $"Exception - [{ex.Message}]"));
                return new EntityResult<ResourceIdViewModel>(request.Notifications, null) { Error = ErrorCode.InternalServerError };
            }
        }
    }
}
