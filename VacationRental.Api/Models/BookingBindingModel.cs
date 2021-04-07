using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Models
{
    public class BookingBindingModel
    {
        [Required]
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        [Range(1, int.MaxValue)]
        public int Nights { get; set; }
    }
}
