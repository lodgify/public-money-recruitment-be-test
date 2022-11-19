using System;
using VacationRental.Application.DTO;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Application.Queries
{
    internal class GetCalendar : IQuery<CalendarDto>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
