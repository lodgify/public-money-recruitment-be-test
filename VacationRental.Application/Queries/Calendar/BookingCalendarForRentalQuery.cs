using System;
using MediatR;
using VacationRental.Application.Queries.Calendar.ViewModel;

namespace VacationRental.Application.Queries.Calendar
{
    public class BookingCalendarForRentalQuery : IRequest<CalendarViewModel>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
