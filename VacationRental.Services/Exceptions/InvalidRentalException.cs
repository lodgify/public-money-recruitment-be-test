using System;
using System.Runtime.Serialization;

namespace VacationRental.Services.Exceptions
{
    [Serializable]
    public class InvalidRentalException : ApplicationException
    {
        public InvalidRentalException()
        {
        }

        protected InvalidRentalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidRentalException(string message) : base(message)
        {
        }

        public InvalidRentalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
