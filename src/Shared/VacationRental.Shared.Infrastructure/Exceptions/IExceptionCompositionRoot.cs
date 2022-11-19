using System;
using VacationRental.Shared.Abstractions.Exceptions;

namespace VacationRental.Shared.Infrastructure.Exceptions;

public interface IExceptionCompositionRoot
{
    ExceptionResponse Map(Exception exception);
}