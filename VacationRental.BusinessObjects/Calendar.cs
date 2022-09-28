using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VacationRental.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class Calendar
    {
        public int RentalId { get; set; }
        public List<CalendarDate> Dates { get; set; }
    }
}
