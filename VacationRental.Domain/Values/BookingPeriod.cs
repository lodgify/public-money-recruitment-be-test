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

        internal BookingPeriod AddNights(int nights) => new BookingPeriod(Start, Nights + nights);

        internal bool IsOverlapped(BookingPeriod periodToCompare) =>  
            (Start >= periodToCompare.GetEndOfPeriod() || periodToCompare.Start >= periodToCompare.GetEndOfPeriod()) == false;

        internal bool Within(DateTime date) => Start <= date.Date && Start.AddDays(Nights) > date.Date;

        private DateTime GetEndOfPeriod() => Start.AddDays(Nights);
    }
}
