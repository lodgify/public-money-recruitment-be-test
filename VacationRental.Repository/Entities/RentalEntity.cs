using System.Diagnostics.CodeAnalysis;
using VacationRental.Repository.Entities.Interfaces;

namespace VacationRental.Repository.Entities
{
    [ExcludeFromCodeCoverage]
    public class RentalEntity : IEntity
    {
        public int Id { get; set; }
        public int Units { get; set; }
    }
}
