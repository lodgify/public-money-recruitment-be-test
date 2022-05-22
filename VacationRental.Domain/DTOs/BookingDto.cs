using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Validations;

namespace VacationRental.Domain.DTOs
{
    public class BookingDto
    {
        public int RentalId { get; set; }
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        [GreaterThanZero(ErrorMessage = "Nights must be positive")]
        public int Nights { get; set; }
        public int Unit { get; set; }
    }
}
