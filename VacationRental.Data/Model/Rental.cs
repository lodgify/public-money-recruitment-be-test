using VacationRental.Data.Model.Abstractions;

namespace VacationRental.Data.Model;

public class Rental : IDataEntity
{
    public int Id { get; set; }
    public int Units { get; set; }
    public int PreparationTimeInDays { get; set; }
}