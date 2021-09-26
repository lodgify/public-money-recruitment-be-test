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


        //Notice: Names of properties can be different and depend on the domain's terminology
        //For instance, for BookingPeriod the 'Nights' property is the period length
        //But for PreparationPeriod it's the 'Days' property

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
