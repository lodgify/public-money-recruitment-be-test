using System.Globalization;
using static System.String;

namespace VacationRental.Api.Exceptions;

public class RentalNotAvailableExcepton : Exception
{
    public RentalNotAvailableExcepton() { }

    public RentalNotAvailableExcepton(string message)
        : base(message) { }

    public RentalNotAvailableExcepton(string message, Exception inner)
        : base(message, inner) { }

    public RentalNotAvailableExcepton(string message, params object[] args)
        : base(Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
