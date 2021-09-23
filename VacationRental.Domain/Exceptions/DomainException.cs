using System;

namespace VacationRental.Domain.Exceptions
{
    /// <summary>
    /// Base exception for all the domain model's exceptions
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
