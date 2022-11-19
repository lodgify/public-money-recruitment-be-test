using System;

namespace VacationRental.Shared.Abstractions.Exceptions
{
    public interface IExceptionToResponseMapper
    {
        ExceptionResponse Map(Exception exception);
    }
}
