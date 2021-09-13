using System.Collections.Generic;
using VacationRental.Infrastructure;

namespace VacationRental.Application
{
    public class GetCalendarResponse : ResponseBase
    {
        public CalendarViewModel CalendarViewModel { get; set; }
    }
}
