using System;

namespace VacationRental.Application
{
    public class AddBookingRequest
    {
        public int RentalId { get; set; }
        public int NumberOfNigths { get; set; }
        public DateTime StartDate { get; set; }
    }
}
