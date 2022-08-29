using System;
using System.Runtime.Serialization;

namespace VacationRental.Services.Exceptions
{
    [Serializable]
    public class BookingNotFoundException : ApplicationException
    {
        public BookingNotFoundException()
        {
        }

        protected BookingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BookingNotFoundException(string message) : base(message)
        {
        }

        public BookingNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
