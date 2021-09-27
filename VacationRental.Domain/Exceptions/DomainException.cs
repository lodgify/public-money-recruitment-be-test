using System;

namespace VacationRental.Domain.Exceptions
{
    /// <summary>
    /// Serves as the base class for domain exceptions
    /// </summary>
    public abstract class DomainException : Exception
    {
        protected DomainException()
        {
            
        }

        protected DomainException(string message) : base(message)
        {

        }
    }
}
