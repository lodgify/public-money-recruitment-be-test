using System.Threading.Tasks;
using VacationRental.Domain.Events.Rental;

namespace VacationRental.Domain.Services
{
    public interface IRentalUpdatedEventHandler
    {
        Task Handle(RentalUpdated eventDetails);
    }
}
