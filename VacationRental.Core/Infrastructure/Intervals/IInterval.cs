using System;

namespace VacationRental.Core.Infrastructure.Intervals
{
    public interface IInterval<T> where T : IComparable<T>
    {
        T Start { get; }

        T End { get; }

        bool Include(T value);

        bool Includes(IInterval<T> interval);
    }
}
