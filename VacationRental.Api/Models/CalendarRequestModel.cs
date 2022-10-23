using System.ComponentModel.DataAnnotations;
using System;

namespace VacationRental.Api.Models
{
    public class CalendarRequestModel
    {
        [Required]
        public int RentalId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public int Nights { get; set; }
    }
}
