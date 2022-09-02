using System;

namespace VR.Application.Queries.GetBooking
{
    public class GetBookingResponse
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
