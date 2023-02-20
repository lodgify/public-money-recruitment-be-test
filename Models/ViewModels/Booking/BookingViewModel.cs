namespace Models.ViewModels.Booking;

public sealed class BookingViewModel
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public int Nights { get; set; }
    public DateTime Start { get; set; }
}
