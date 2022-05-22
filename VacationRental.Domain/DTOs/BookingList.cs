using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Domain.DTOs
{
    public class BookingList
    {
        public List<DateTime> StartDates { get; set; }
        public List<DateTime> EndDates { get; set; }

    }
}
