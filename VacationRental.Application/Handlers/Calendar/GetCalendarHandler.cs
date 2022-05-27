using Flunt.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Notifications;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Persistance;
using VacationRental.Persistance.Interfaces;

namespace VacationRental.Application.Handlers.Calendar
{
    public class GetCalendarHandler : IRequestHandler<GetCalendarRequest, EntityResult<CalendarViewModel>>
    {
        private readonly IRepository<RentalEntity> _rentalRepository;
        private readonly IRepository<BookingEntity> _bookingRepository;

        public GetCalendarHandler(IRepository<RentalEntity> repository, IRepository<BookingEntity> bookingRepository)
        {
            _rentalRepository = repository;
            _bookingRepository = bookingRepository;
        }

        public async Task<EntityResult<CalendarViewModel>> Handle(GetCalendarRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entityResult = new EntityResult<CalendarViewModel>(request.Notifications, null);

                var bookings = _bookingRepository.GetAll();

                if (entityResult.Valid)
                {
                    var rental = _rentalRepository.GetById(request.RentalId);

                    if (rental == null)
                    {
                        return entityResult;
                    }

                    var result = new CalendarViewModel
                    {
                        RentalId = request.RentalId,
                        Dates = new List<CalendarDateViewModel>()
                    };
                    for (var i = 0; i < request.Nights; i++)
                    {
                        var date = new CalendarDateViewModel
                        {
                            Date = request.Start.Date.AddDays(i),
                            Bookings = new List<CalendarBookingViewModel>()
                        };

                        foreach (var booking in bookings)
                        {
                            if (booking.RentalId == request.RentalId
                                && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                            {
                                date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                            }
                        }

                        result.Dates.Add(date);
                    }

                    return new EntityResult<CalendarViewModel>(request.Notifications, result);

                }

                return entityResult;
            }
            catch (Exception ex)
            {
                request.AddNotification(new Notification("GetCalendarHandler-Handler", $"Exception - [{ex.Message}]"));
                return new EntityResult<CalendarViewModel>(request.Notifications, null) { Error = ErrorCode.InternalServerError };
            }
        }

       
    }
}
