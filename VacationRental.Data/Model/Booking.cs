using VacationRental.Data.Model.Abstractions;

namespace VacationRental.Data.Model;

public class Booking : IDataEntity
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public DateTime Start { get; set; }
    public int Nights { get; set; }
}

