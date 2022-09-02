using System;

namespace VR.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int RentalId { get; set; }

        public Rental Rental { get; set; }

        public int Unit { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public DateTime End { get => Start.AddDays(Nights); }
        public int Nights { get; set; }
    }
}
