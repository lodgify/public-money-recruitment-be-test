namespace VacationRental.Application.ViewModels
{
    public class RentalViewModel
    {
        public RentalViewModel(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public int Units { get; set; }
    }
}
