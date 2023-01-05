using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Contracts.Mediatr;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Aggregates.Calendars;
using VacationRental.Domain.Messages.Calendars;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar
{
    public class GetRentalCalendarQueryHandler : IQueryHandler<GetRentalCalendarQuery, CalendarDto>
    {
        private readonly IRepository<Rental> _rentalRepository;
        private readonly IBookingRepository _bookingRepository;        

        public GetRentalCalendarQueryHandler(IRepository<Rental> rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;            
        }

        public Task<CalendarDto> Handle(GetRentalCalendarQuery request, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.GetById(request.RentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var result = new CalendarDto
            {
                RentalId = rental.Id,
                Dates = new List<CalendarDate>()
            };

            var bookings = _bookingRepository.GetBookingByRentalId(request.RentalId);

            for (var i = 0; i < request.Nights; i++)
            {
                var date = new CalendarDate
                {
                    Date = request.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBooking>(),
                    PreparationTimes = new List<PreparationTime>()
                };

                foreach (var booking in bookings)
                {
                    date.AggregateBookings(booking);
                    date.DefinePreparationTimes(booking, rental.PreparationTimeInDays);                    
                }

                result.Dates.Add(date);
            }

            return Task.FromResult(result);
        }
    }
}
