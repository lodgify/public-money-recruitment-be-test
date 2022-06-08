using System.Runtime.Serialization;

namespace VacationRental.Models.Exceptions
{
    [Serializable]
    public class RentalInvalidException : ApplicationException
    {
        public RentalInvalidException()
        { }

        public RentalInvalidException(string message) : base(message)
        { }

        public RentalInvalidException(string message, Exception inner) : base(message, inner)
        { }

        public RentalInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
