using System;

namespace VacationRental.Business
{
    public class Booking : EntityBase
    {        
        public Rental Rental { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfNights { get; set; }
    }
}
