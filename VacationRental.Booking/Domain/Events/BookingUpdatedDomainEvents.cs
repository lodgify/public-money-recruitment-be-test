using VacationRental.Booking.Domain.Interfaces;
using VacationRental.Domain.Base;
using VacationRental.Rental.Domain.Interfaces;

namespace VacationRental.Booking.Entities.Rentals.Events
{
    public class OnBookingUpdatedDomainEvent : BaseDomainEvent
    {

        public OnBookingUpdatedDomainEvent(IBookingEntity booking)
        {
            this.Booking = booking;
        }

        public IBookingEntity Booking { get; }
    }

    public class OnBookingCreatedDomainEvent : BaseDomainEvent
    {

        public OnBookingCreatedDomainEvent(IBookingEntity booking, IRental rental)
        {
            this.Booking = booking;
            this.Rental = rental;
        }

        public IBookingEntity Booking { get; }
        public IRental Rental { get; }
    }
}