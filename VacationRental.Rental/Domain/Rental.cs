using VacationRental.Domain.Base;
using VacationRental.Rental.Domain.Interfaces;

namespace VacationRental.Rental.Domain
{
    public partial class Rental : BaseEntity, IRentalEntity
    {
        public int PreparationTimeInDays { get; set; }
        public int Units { get; set; }
    }
}