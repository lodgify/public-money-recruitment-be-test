using System;

namespace VacationRental.Application.DTO
{
    internal class BookingDto
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
