using System;
using System.Runtime.Serialization;

namespace VacationRental.Services.Exceptions
{
    [Serializable]
    public class RentalNotFoundException : ApplicationException
    {
        public RentalNotFoundException()
        {
        }

        public RentalNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RentalNotFoundException(string message) : base(message)
        {
        }

        public RentalNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
