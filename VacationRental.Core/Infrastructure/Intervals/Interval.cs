using System;

namespace VacationRental.Core.Infrastructure.Intervals
{
    public class Interval<T> : IInterval<T> where T : IComparable<T>
    {
        public Interval(T start, T end)
        {
            Start = start;
            End = end;
        }

        public T Start { get; }

        public T End { get; }

        public bool Include(T value)
        {
            return Start.CompareTo(value) <= 0 && End.CompareTo(value) >= 0;
        }

        public bool Includes(IInterval<T> interval)
        {
            return Start.CompareTo(interval.Start) <= 0 && End.CompareTo(interval.End) >= 0;
        }

        public override string ToString()
        {
            return $"{Start} - {End}";
        }
    }
}
