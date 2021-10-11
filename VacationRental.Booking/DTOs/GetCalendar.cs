using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VacationRental.Booking.Domain;
using VacationRental.Booking.Domain.Interfaces;

namespace VacationRental.Booking.DTOs
{
    public class GetCalendarResponse : Domain.Calendar
    {
        public GetCalendarResponse(int rentalId, IList<CalendarDay> dates) : base(rentalId, dates)
        {
        }
    }

    public class GetCalendarRequest : IBooking
    {
        [Required]
        public int RentalId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public int Nights { get; set; }

        public int Id { get; set; }
        public int PreparationTime { get; set; }
        public int Unit { get; set; }
    }
}