using System;

namespace VacationRental.Core
{
    public class Booking
    {
        public int Id { get; set; }

        public int RentalId { get; set; }

        private DateTime _startIgnoreTime;
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        public int Nights { get; set; }

        public int Unit { get; set; }
    }
}
