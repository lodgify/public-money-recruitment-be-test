namespace Application.Models.Rental.Requests;

public class CreateRentalRequest
{
    public int Units { get; set; }

    public int PreparationTimeInDays { get; set; }
}