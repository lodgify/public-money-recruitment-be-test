namespace Application.Models.Booking.Requests;

public class UpdateBookingRequest
{    
    
    public int Id { get; set; }
    public int RentalId { get; set; }

    public DateTime Start
    {
        get => _startIgnoreTime;
        set => _startIgnoreTime = value.Date;
    }

    private DateTime _startIgnoreTime;
    public int Nights { get; set; }
    public int Unit { get; set; }
    
    public bool IsPreparation { get; set; }
}