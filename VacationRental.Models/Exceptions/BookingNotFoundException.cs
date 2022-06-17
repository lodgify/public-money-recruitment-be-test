using System.Runtime.Serialization;

namespace VacationRental.Models.Exceptions
{
    [Serializable]
    public class BookingNotFoundException : ApplicationException
    {
        public BookingNotFoundException() 
        { }

        public BookingNotFoundException(string message) : base(message)
        { }

        public BookingNotFoundException(string message, Exception inner) : base(message, inner)
        { }

        public BookingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
