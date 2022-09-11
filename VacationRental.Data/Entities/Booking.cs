using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VacationRental.Data.Entities
{
    public class Booking : BaseEntity
    {
        [ForeignKey("Rental")]
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public virtual Rental Rental { get; set; }
    }
}
