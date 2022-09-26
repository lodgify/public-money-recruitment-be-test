using System.Diagnostics.CodeAnalysis;

namespace VacationRental.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class Rental
    {
        public int Id { get; set; }
        public int Units { get; set; }
    }
}
