namespace Domain.Entities;

public class Booking : BaseEntity
{

    public int RentalId { get; set; }
    public int Unit { get; set; }
    public DateTime Start { get; set; }
    public int Nights { get; set; }
    public DateTime LastDay { get; set; }
    public bool IsPreparation { get; set; }
    
    
}