namespace Models.DataModels;

public sealed class RentalDto
{
    public int Id { get; set; } 
    public int PreparationTimeInDays { get; set; }
    public List<int>? Units { get; set; }
}
