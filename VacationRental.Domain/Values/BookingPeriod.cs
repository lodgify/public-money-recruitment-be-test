using System;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Domain.Values
{
    public sealed class BookingPeriod : TimePeriod
    {
        public BookingPeriod(DateTime start, int nights) : base(start, nights)
        {
            ThrowIfNightsLessThanOne(nights);
        }

        public DateTime Start => GetStart();
        public int Nights => GetDays();

        private static void ThrowIfNightsLessThanOne(int nights)
        {
            if (nights < 1)
            {
                throw new NightsLessThanOneException();
            }
        }
    }
}
