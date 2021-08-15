namespace VacationRental.Core
{
    public interface IRentalRepository
    {
        Rental Get(int id);
        int Create(Rental rental);
    }
}
