using System;
using System.ComponentModel.DataAnnotations;
using VacationRental.Booking.Domain.Interfaces;

namespace VacationRental.Booking.DTOs
{
    public class GetBookingResponse : IBooking
    {
        public int Id { get; set; }
        public int Unit { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int PreparationTime { get; set; }
    }

    public class GetBookingRequest : IBookingId
    {
        [Required]
        public int Id { get; set; }
    }
}