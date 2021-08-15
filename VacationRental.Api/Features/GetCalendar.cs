using MediatR;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Core;

namespace VacationRental.Api.Features
{
    public class GetCalendar
    {
        public class Query : IRequest<CalendarViewModel>
        {
            public int Nights { get; set; }
            public DateTime Start { get; set; }
            public int RentalId { get; set; }
        }

        public class QueryHandler : RequestHandler<Query, CalendarViewModel>
        {
            private readonly IRentalRepository _rentals;
            private readonly IBookingRepository _bookings;

            public QueryHandler(IRentalRepository rentals, IBookingRepository bookings)
            {
                _rentals = rentals;
                _bookings = bookings;
            }

            protected override CalendarViewModel Handle(Query request)
            {
                if (request.Nights < 0)
                    throw new ApplicationException("Nights must be positive");

                Rental rental = _rentals.Get(request.RentalId);

                if (rental is null)
                    throw new ApplicationException("Rental not found");

                var result = new CalendarViewModel
                {
                    RentalId = request.RentalId,
                    Dates = new List<CalendarDateViewModel>()
                };

                List<Booking> bookings = _bookings.GetAll();

                for (var i = 0; i < request.Nights; i++)
                {
                    var date = new CalendarDateViewModel
                    {
                        Date = request.Start.Date.AddDays(i),
                        Bookings = new List<CalendarBookingViewModel>(),
                        PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    };

                    foreach (var booking in bookings)
                    {
                        DateTime bookingEndDate = booking.Start.AddDays(booking.Nights);
                        if (booking.RentalId == request.RentalId
                            && booking.Start <= date.Date)
                        {
                            if (bookingEndDate > date.Date)
                            {
                                date.Bookings.Add(new CalendarBookingViewModel
                                {
                                    Id = booking.Id,
                                    Unit = booking.Unit
                                });
                            }
                            else if (bookingEndDate.AddDays(rental.PreparationTimeInDays) > date.Date)
                            {
                                date.PreparationTimes.Add(new CalendarPreparationTimeViewModel
                                {
                                    Unit = booking.Unit
                                });
                            }
                        }
                    }

                    result.Dates.Add(date);
                }

                return result;
            }
        }
    }
}
