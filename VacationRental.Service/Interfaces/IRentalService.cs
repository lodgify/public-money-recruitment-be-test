namespace VacationRental.Application
{
    public interface IRentalService
    {
        UpdateRentalResponse UpdateRental(UpdateRentalRequest request);

        GetRentalResponse GetRental(GetRentalRequest request);

        AddRentalResponse AddRental(AddRentalRequest request);
    }
}
