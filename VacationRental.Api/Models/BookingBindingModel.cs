using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Models
{
    public class BookingBindingModel
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;

        [Range(1, int.MaxValue)]
        public int Nights { get; set; }
    }
}
