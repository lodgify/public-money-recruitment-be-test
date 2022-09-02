using System.Collections.Generic;
using VR.Application.Queries.GetCalendar.ViewModels;

namespace VR.Application.Queries.GetCalendar
{
    public class GetCalendarResponse
    {
        public int RentalId { get; set; }

        public IList<CalendarDateViewModel> Dates { get; set; }
    }
}
