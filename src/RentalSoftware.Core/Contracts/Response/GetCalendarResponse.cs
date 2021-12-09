using RentalSoftware.Core.Entities;

namespace RentalSoftware.Core.Contracts.Response
{
    public class GetCalendarResponse : ResponseBase
    {
        public CalendarViewModel CalendarViewModel { get; set; }
    }
}
