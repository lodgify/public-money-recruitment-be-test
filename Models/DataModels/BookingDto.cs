namespace Models.DataModels;

public sealed class BookingDto
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public int Unit { get; set; }
    public DateTime Start { get; set; }
    public int Nights { get; set; }
    public int PreparationTimeInDays { get; set; }
}
