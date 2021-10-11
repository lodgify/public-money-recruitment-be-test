using System;
using System.ComponentModel.DataAnnotations;
using VacationRental.Booking.Domain.Interfaces;

namespace VacationRental.Booking.DTOs
{
    public class AddBookingRequest : IBooking
    {
        [Required]
        public int RentalId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public int Nights { get; set; }

        public int Unit { get; set; }
        public int Id { get; }
        public int PreparationTime { get; set; }
    }

    public class AddBookingResponse : IBookingId
    {
        public int Id { get; set; }
    }
}