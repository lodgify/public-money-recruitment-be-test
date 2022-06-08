using System.Runtime.Serialization;

namespace VacationRental.Models.Exceptions
{
    [Serializable]
    public class RentalNotFoundException : ApplicationException
    {
        public RentalNotFoundException() 
        { }

        public RentalNotFoundException(string message) : base(message)
        { }

        public RentalNotFoundException(string message, Exception inner) : base(message, inner)
        { }

        public RentalNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
