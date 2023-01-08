using System;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Domain.Messages.Calendars;

namespace VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar
{
    public sealed record class GetRentalCalendarQuery(int RentalId, DateTime Start, int Nights) : IQuery<CalendarDto>
    {
    }
}
