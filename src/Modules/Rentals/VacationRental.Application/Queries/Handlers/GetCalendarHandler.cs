using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.DTO;
using VacationRental.Application.Exceptions;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Application.Queries.Handlers
{
    internal class GetCalendarHandler : IQueryHandler<GetCalendar, CalendarDto>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public GetCalendarHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public Task<CalendarDto> HandleAsync(GetCalendar query, CancellationToken cancellationToken = default)
        {
            if (query.Nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalRepository.Get(query.RentalId);

            if (rental is null)
            {
                throw new RentalNotFoundException(query.RentalId);
            }

            var rentalBookings = _bookingRepository.GetAll(query.RentalId);

            var result = new CalendarDto
            {
                RentalId = query.RentalId,
                Dates = new List<CalendarDateDto>()
            };

            for (var i = 0; i < query.Nights; i++)
            {
                var date = new CalendarDateDto
                {
                    Date = query.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingDto>()
                };

                foreach (var booking in rentalBookings)
                {
                    if (booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingDto { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return Task.FromResult(result);
        }
    }
}
