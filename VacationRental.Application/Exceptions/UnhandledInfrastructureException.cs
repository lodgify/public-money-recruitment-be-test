using System;

namespace VacationRental.Application.Exceptions
{
    public sealed class UnhandledInfrastructureException : ApplicationException
    {

        public UnhandledInfrastructureException() : base("Unhandled error occurred")
        {
            
        }
    }
}
