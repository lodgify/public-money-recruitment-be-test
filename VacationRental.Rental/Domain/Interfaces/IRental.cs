using VacationRental.Domain.Interfaces;

namespace VacationRental.Rental.Domain.Interfaces
{
    public interface IRentalId
    {
        int Id { get; }

    }
    public interface IRental : IRentalId
    {
        int Units { get; set; }
        int PreparationTimeInDays { get; set; }
    }
    public interface IRentalEntity : IBaseEntity, IRental
    {

    }
}
