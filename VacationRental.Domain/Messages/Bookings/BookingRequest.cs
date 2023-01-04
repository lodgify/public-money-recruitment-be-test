using System;

namespace VacationRental.Domain.Messages.Bookings
{
    public class BookingRequest
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }
        public int Units { get; set; }

    }
}
