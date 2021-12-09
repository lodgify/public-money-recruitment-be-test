using RentalSoftware.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalSoftware.Core.Entities
{
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Units { get; set; }
        public RentalType RentalType { get; set; }
        public List<Booking> Bookings { get; set; }
        public int PreparationTime { get; set; }
    }
}
