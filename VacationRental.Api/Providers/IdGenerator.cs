using System.Collections.Generic;

namespace VacationRental.Api.Providers;

public class IdGenerator : IIdGenerator
{
    public int Generate<T>(HashSet<T> set)
    {
        return set.Count + 1;
    }
}