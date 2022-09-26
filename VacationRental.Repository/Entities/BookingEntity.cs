using System;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Repository.Entities.Interfaces;

namespace VacationRental.Repository.Entities
{
    [ExcludeFromCodeCoverage]
    public class BookingEntity : IEntity
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
