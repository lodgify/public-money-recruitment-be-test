namespace VacationRental.Domain.Rentals
{
    public interface IRentalRepository
    {
        int Save(RentalModel model);
        RentalModel Get(int id);
        
        int GetLastId();
    }
}