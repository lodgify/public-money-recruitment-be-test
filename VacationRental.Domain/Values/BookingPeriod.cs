using System;
using System.Collections.Generic;
using VacationRental.Domain.Common;

namespace VacationRental.Domain.Values
{
    public class BookingPeriod : ValueObject<BookingPeriod>
    {
        public BookingPeriod(DateTime start, int nights)
        {
            Start = start;
            Nights = nights;
        }

        public DateTime Start { get; } 
        public int Nights { get; }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>{Start, Nights};
        }
    }
}
