using System.Runtime.Serialization;

namespace VacationRental.Models.Exceptions
{
    [Serializable]
    public class BookingInvalidException : ApplicationException
    {
        public BookingInvalidException()
        { }

        public BookingInvalidException(string message) : base(message)
        { }

        public BookingInvalidException(string message, Exception inner) : base(message, inner)
        { }

        public BookingInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
