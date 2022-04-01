namespace VacationRental.Domain.Rentals
{
    public interface IRentalRepository
    {
        int Add(RentalModel model);
        
        int Update(RentalModel model);
        RentalModel Get(int id);
    }
}
