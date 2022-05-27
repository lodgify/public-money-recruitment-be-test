using System;

namespace VacationRental.Domain.Entities
{
    public class RentalEntity
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
