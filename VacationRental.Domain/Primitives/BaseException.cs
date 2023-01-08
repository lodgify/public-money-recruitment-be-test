using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Domain.Primitives
{
    public class BaseException : ApplicationException
    {
        public DomainError Error { get; }

        public BaseException(DomainError error) : base(error.Message)
        {
            Error = error;
        }

        public BaseException()
        {
            Error = new DomainError("\"One or more errors has been found\"");
        }
    }
}
