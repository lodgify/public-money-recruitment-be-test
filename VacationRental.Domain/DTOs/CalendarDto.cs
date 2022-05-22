using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Validations;

namespace VacationRental.Domain.DTOs
{
    public class CalendarDto
    {
        public int rentalId { get; set; }
        public DateTime start { get; set; }
        [GreaterThanZero(ErrorMessage = "Nights must be positive")]
        public int nights { get; set; }
    }
}
