using System;

namespace VacationRental.Api.Core.Helpers.Exceptions
{
    public class NotFoundException : Exception
    {
        public int RentalId { get; set; }
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
        public NotFoundException(string message, int rentalId) : this(message)
            => RentalId = rentalId;
    }
}
