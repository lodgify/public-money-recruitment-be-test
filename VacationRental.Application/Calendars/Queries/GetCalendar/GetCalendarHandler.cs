using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VacationRental.Application.Calendars.Models;
using VacationRental.Domain;

namespace VacationRental.Application.Calendars.Queries.GetCalendar
{
    public class GetCalendarHandler : IRequestHandler<GetCalendarQuery, CalendarViewModel>
    {
        private readonly VacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCalendarHandler(VacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            var rental = await _dbContext.Rentals
                .AsNoTracking()
                .Include(r => r.Units)
                .ThenInclude(u => u.Bookings.Where(b => b.Start < request.Start.AddDays(b.Unit.Rental.PreparationTimeInDays + request.Nights) && request.Start < b.End.AddDays(b.Unit.Rental.PreparationTimeInDays)))
                .FirstOrDefaultAsync(b => b.Id == request.RentalId, cancellationToken);

            if (rental == null)
            {
                throw new ApplicationException("Rental not found");
            }

            var result = new CalendarViewModel(request.RentalId);

            for (var i = 0; i < request.Nights; i++)
            {
                var date = new CalendarDateViewModel(request.Start.Date.AddDays(i));

                foreach (var booking in rental.Units.SelectMany(b => b.Bookings))
                {
                    if (booking.Start <= date.Date && date.Date < booking.End)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel(booking.Id, booking.UnitId));
                    }
                    if (booking.End <= date.Date && date.Date < booking.End.AddDays(rental.PreparationTimeInDays))
                    {
                        date.PreparationTimes.Add(new CalendarPreparationTimeViewModel(booking.UnitId));
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
