using System;

namespace VacationRental.Application.Exceptions
{
    /// <summary>
    /// Serves as the base class for infrastructure exceptions
    /// </summary>
    public abstract class InfrastructureException : Exception
    {
        protected InfrastructureException()
        {
            
        }

        protected InfrastructureException(string message) : base(message)
        {

        }

        protected InfrastructureException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
