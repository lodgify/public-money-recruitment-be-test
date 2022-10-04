using MediatR;
using System.ComponentModel.DataAnnotations;
using VacationRental.Application.Calendars.Models;

namespace VacationRental.Application.Calendars.Queries.GetCalendar
{
    public class GetCalendarQuery : IRequest<CalendarViewModel>
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Nights must be positive")]
        public int Nights { get; set; }
    }
}
