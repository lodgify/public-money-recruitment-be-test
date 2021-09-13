using VacationRental.Infrastructure;

namespace VacationRental.Application
{
    public class GetRentalResponse : ResponseBase
    {
        public RentalViewModel RentalViewModel { get; set; }
    }
}
