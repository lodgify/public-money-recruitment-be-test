namespace Domain.DAL.Models
{
    public class Rental : BaseEntity
    {
        public int Units { get; set; }
        
        public int PreparationTimeInDays { get; set; }
    }
}
