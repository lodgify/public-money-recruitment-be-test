using VacationRental.Data.Model.Abstractions;
using VacationRental.Data.Model.Enums;

namespace VacationRental.Data.Model;

public class Booking : IDataEntity
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public DateTime Start { get; set; }
    public int Nights { get; set; }
    public int Unit { get; set; }
    public BookingType Type { get; set; }
}

