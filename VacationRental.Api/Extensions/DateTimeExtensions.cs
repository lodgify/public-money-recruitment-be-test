using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static ICollection<DateTime> CreateRangeUntil(this DateTime start, int quantity) =>
                    Enumerable.Range(0, quantity).Select(i => i == 0 ? start : start.AddDays(i)).ToList();

        public static ICollection<DateOnly> CreateDateOnlyRangeUntil(this DateTime start, int quantity) =>
                    Enumerable.Range(0, quantity).Select(i => i == 0 ? DateOnly.FromDateTime(start) : DateOnly.FromDateTime(start.AddDays(i))).ToList();
    }
}

