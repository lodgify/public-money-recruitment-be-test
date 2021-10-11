using MediatR;
using VacationRental.Domain.Base;
using VacationRental.Rental.Domain.Interfaces;

namespace VacationRental.Rental.Entities.Rentals.Events
{
    public class OnRentalUpdatedDomainEvent : BaseDomainEvent, INotification
    {

        public OnRentalUpdatedDomainEvent(IRental rental)
        {
            this.Rental = rental;
        }

        public IRental Rental { get; }
    }
}