using System.Collections.Generic;

namespace VacationRental.Api.Providers;

public interface IIdGenerator
{
    int Generate<T>(HashSet<T> set);
}