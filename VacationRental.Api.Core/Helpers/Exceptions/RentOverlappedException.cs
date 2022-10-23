using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Api.Core.Helpers.Exceptions
{
    public class RentOverlappedException : Exception
    {
        public int RentId { get; set; }
        public RentOverlappedException() { }
        public RentOverlappedException(string message) : base(message) { }
        public RentOverlappedException(string message, int rentId) : this(message) 
        {
            RentId = rentId;
        }
    }
}
