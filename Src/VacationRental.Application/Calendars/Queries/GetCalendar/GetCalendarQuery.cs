using System;
using MediatR;

namespace VacationRental.Application.Calendars.Queries.GetCalendar
{
    public class GetCalendarQuery : IRequest<CalendarViewModel>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}