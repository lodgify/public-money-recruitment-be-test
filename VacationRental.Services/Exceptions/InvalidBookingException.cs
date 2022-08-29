using System;
using System.Runtime.Serialization;

namespace VacationRental.Services.Exceptions
{
    [Serializable]
    public class InvalidBookingException : ApplicationException
    {
        public InvalidBookingException()
        {
        }

        protected InvalidBookingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidBookingException(string message) : base(message)
        {
        }

        public InvalidBookingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
