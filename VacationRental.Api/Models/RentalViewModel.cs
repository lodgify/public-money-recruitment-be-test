using System.Diagnostics.CodeAnalysis;

namespace VacationRental.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class RentalViewModel
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
