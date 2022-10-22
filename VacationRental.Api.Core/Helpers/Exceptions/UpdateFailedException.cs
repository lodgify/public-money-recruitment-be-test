using System;

namespace VacationRental.Api.Core.Helpers.Exceptions
{
    public class UpdateFailedException : Exception
    {
        public int RentalId { get; set; }

        public UpdateFailedException() { }
        public UpdateFailedException(string message) : base(message) { }
        public UpdateFailedException(string message, int rentalId) : this(message)
        {
            RentalId = rentalId;
        }
    }
}
