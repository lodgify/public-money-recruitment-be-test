namespace VacationRental.Application.Queries.Rental
{
    public sealed class GetRentalById
    {
        public GetRentalById(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
