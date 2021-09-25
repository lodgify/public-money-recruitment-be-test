using System;

namespace VacationRental.Domain.Values
{
    public sealed class PreparationPeriod : TimePeriod
    {
        public PreparationPeriod(DateTime start, int days) : base(start, days)
        {

        }

        public DateTime Start => GetStart();
        public int Days => GetDays();
    }
}
