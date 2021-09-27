using System.Threading.Tasks;

namespace VacationRental.Domain.Events.Rental
{
    public delegate Task RentalUpdatedHandler(RentalUpdated rentalUpdatedDetails);
}

