using System.Threading.Tasks;
using VacationRental.Domain.Events.Rental;


public delegate Task RentalUpdatedHandler(RentalUpdated rentalUpdatedDetails);

