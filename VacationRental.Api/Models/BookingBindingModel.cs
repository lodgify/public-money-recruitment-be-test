using System;

namespace VacationRental.Api.Models
{
    //[Index(nameof(Token), IsUnique = true)]
    public class BookingBindingModel
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public DateTime End => this.Start.AddDays(this.Nights);
        public int Nights { get; set; }
        
        public int Units { get; set; }

        public int BookingId { get; set; }

        //public string Token { get; set; }
    }
}
