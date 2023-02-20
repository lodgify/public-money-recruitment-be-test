using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Booking;

public sealed class BookingBindingViewModel
{
    public int RentalId { get; set; }

    public int Nights { get; set; }

    public DateTime Start
    {
        get => _startIgnoreTime;
        set => _startIgnoreTime = value.Date;
    }

    private DateTime _startIgnoreTime;
}