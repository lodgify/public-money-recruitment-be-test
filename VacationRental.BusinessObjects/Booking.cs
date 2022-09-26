using System;
using System.Diagnostics.CodeAnalysis;

namespace VacationRental.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class Booking
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
