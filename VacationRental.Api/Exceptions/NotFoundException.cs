using System.Globalization;
using static System.String;

namespace VacationRental.Api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() { }

    public NotFoundException(string message)
        : base(message) { }

    public NotFoundException(string message, Exception inner)
        : base(message, inner) { }

    public NotFoundException(string message, params object[] args)
        : base(Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
