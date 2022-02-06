using System;

namespace Application.Models.Booking.Responses
{
    public class BookingResponse
    {
        public int RentalId { get; set; }
        
        public int Unit { get; set; }
        
        public DateTime Start { get; set; }
        
        public int Nights { get; set; }
        
        public bool IsPreparation { get; set; }
    }
}