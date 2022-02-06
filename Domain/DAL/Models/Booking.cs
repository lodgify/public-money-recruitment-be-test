using System;

namespace Domain.DAL.Models
{
    public class Booking : BaseEntity
    {
        public int RentalId { get; set; }
        
        public int Unit { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        
        public DateTime LastDay => Start.AddDays(Nights);
        
        public bool IsPreparation { get; set; }
    }
}
