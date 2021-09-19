using System;

namespace VacationRental.Api.Library
{
    public static class Extensions
    {
        public static bool IsBewteenTwoDates(this (DateTime startThersholdDate, DateTime endThersholdDate) dates, DateTime start, DateTime end)
        {
            var isLaterThenStartDates = dates.startThersholdDate >= start && dates.startThersholdDate <= end;
            var isEarlierThenStartDates = dates.endThersholdDate >= start && dates.endThersholdDate <= end;
            return isLaterThenStartDates && isEarlierThenStartDates;
        }
    }
}
