using VacationRental.Business;

namespace VacationRental.Application
{
    public static class RentalMapper
    {
        public static RentalViewModel ConvertToRentalViewModel(this Rental rental)
        {
            RentalViewModel rentalViewModel = new RentalViewModel();

            rentalViewModel.Id = rental.Id;
            rentalViewModel.PreparationTimeInDays = rental.PreparationTimeInDays;
            rentalViewModel.Units = rental.Units;
            
            return rentalViewModel;
        }     
    }
}