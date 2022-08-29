using System.Collections.Generic;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Services.Rentals
{
    public interface IRentalService
    {
        IEnumerable<RentalDto> GetRentals();

        RentalDto GetRentalBy(int rentalId);

        RentalDto AddRental(CreateRentalRequest request);
        
        RentalDto UpdateRental(int rentalId, CreateRentalRequest request);
        
        bool DeleteRental(int rentalId);
    }
}
