using System;

namespace VacationRental.Domain.Bookings
{
    public class BookingModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public DateTime End => Start.AddDays(Nights);
        public int Nights { get; set; }
    }
}
