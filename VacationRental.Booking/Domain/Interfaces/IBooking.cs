using System;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Booking.Domain.Interfaces
{
    public interface IBookingId
    {
        int Id { get; }
    }

    public interface IBookingUnit
    {
        int Unit { get; set; }
    }

    public interface IBookingPreparation
    {
        int PreparationTime { get; set; }
    }

    public interface IBooking : IBookingId, IBookingPreparation, ICalendarBooking
    {
        int RentalId { get; set; }
        DateTime Start { get; set; }
        public int Nights { get; set; }
    }

    public interface IBookingEntity : IBaseEntity, IBooking
    {

    }
}
