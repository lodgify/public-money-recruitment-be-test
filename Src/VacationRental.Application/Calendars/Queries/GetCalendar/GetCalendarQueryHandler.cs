using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Calendars.Queries.GetCalendar
{
    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, CalendarViewModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        public GetCalendarQueryHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.Get(request.RentalId);

            if (rental == null)
                return null;

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

                foreach (var booking in _bookingRepository.GetAll())
                {
                    if (booking.RentalId == request.RentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel {Id = booking.Id});
                    }
                }

                result.Dates.Add(date);
            }

            return await Task.FromResult(result);
        }
    }
}