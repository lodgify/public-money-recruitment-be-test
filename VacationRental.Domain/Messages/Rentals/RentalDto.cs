using System;

namespace VacationRental.Domain.Messages.Rentals
{
    public class RentalDto
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
