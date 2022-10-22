using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Api.Core.Helpers.Exceptions
{
    public class NotAvailableException : Exception
    {
        public int RentalId { get; set; }
        public NotAvailableException() { }
        public NotAvailableException(string message) : base(message) { }
        public NotAvailableException(string message, int rentalId) : this(message) 
        {
            RentalId = rentalId;
        }
    }
}
