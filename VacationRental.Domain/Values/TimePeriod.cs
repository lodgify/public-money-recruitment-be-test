using System;
using System.Collections.Generic;
using VacationRental.Domain.Common;

namespace VacationRental.Domain.Values
{
    public abstract class TimePeriod : ValueObject
    {
        private DateTime _start { get; }
        private int _days { get; }

        protected TimePeriod(DateTime date, int days)
        {
            _start = date.Date; //Time is not counted
            _days = days;
        }

        protected DateTime GetStart() => _start;
        protected int GetDays() => _days;

        internal bool Within(DateTime date) => _start <= date.Date && _start.AddDays(_days) > date.Date;

        internal bool IsOverlapped(TimePeriod periodToCompare) =>
            (_start >= periodToCompare.GetEndOfPeriod() || periodToCompare._start >= periodToCompare.GetEndOfPeriod()) == false;

        internal DateTime GetEndOfPeriod() => _start.AddDays(_days);


        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { _start, _days };
        }
    }
}
