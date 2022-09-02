using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VR.Application.Base;
using VR.Application.Queries.GetCalendar.ViewModels;
using VR.DataAccess;
using VR.Domain.Models;
using VR.Infrastructure.Exceptions;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Queries.GetCalendar
{
    public class GetCalendarQueryHandler : BaseRequestHandler<GetCalendarQuery, GetCalendarResponse>
    {
        public GetCalendarQueryHandler(IObjectMapper mapper, VRContext context) : base(mapper, context) { }

        public async override Task<GetCalendarResponse> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            if (_context.Rentals.Find(request.RentalId) == null)
                throw new NotFoundException("Rental is not found", $"GetCalendarQuery - rental with id {request.RentalId} not found");

            var bookings = _context.Bookings.Where(x => x.RentalId == request.RentalId);

            var calendar = GenerateCalendar(request.RentalId, request.Start, request.Nights, bookings);

            return calendar;
        }

        private GetCalendarResponse GenerateCalendar(int rentalId, DateTime start, int nights, IEnumerable<Booking> bookings)
        {
            var calendar = new GetCalendarResponse
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var preparationDays = _context.Rentals.Find(rentalId).PreparationTimeInDays;

            for (var i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var calendarBookings = GetCalendarBookings(bookings, date);
                var calendarPreparationTimes = GetCalendarPreparationTimes(bookings, date, preparationDays);

                calendar.Dates.Add(new CalendarDateViewModel
                {
                    Date = date,
                    Bookings = calendarBookings,
                    PreparationTimes = calendarPreparationTimes
                });
            }

            return calendar;
        }

        private List<CalendarBookingViewModel> GetCalendarBookings(IEnumerable<Booking> bookings, DateTime date)
        {
            var result = bookings
                .Where(
                    p => p.Start <= date.Date &&
                         p.End > date.Date)
                .Select(p => new CalendarBookingViewModel
                {
                    Id = p.Id,
                    Unit = p.Unit,
                }).ToList();
            return result;
        }

        private List<PreparationTimeViewModel> GetCalendarPreparationTimes(IEnumerable<Booking> bookings, DateTime date, int preparationDays)
        {
            return bookings
                    .Where(
                        p => p.End <= date.Date &&
                        p.End.AddDays(preparationDays) > date.Date)
                    .Select(p => new PreparationTimeViewModel
                    {
                        Unit = p.Unit,
                    }).ToList();
        }
    }
}