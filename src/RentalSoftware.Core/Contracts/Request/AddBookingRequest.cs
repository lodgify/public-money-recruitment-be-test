using System;

namespace RentalSoftware.Core.Contracts.Request
{
    public class AddBookingRequest
    {
        public int RentalId { get; set; }
        public int Nights { get; set; }
        public DateTime StartDate { get; set; }
    }
}
