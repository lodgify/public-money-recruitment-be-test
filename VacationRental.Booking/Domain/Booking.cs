using System;
using VacationRental.Booking.Domain.Interfaces;
using VacationRental.Domain.Base;

namespace VacationRental.Booking.Domain
{
    public partial class Booking : BaseEntity, IBookingEntity
    {
        public int Unit { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int PreparationTime { get; set; }


    }
}