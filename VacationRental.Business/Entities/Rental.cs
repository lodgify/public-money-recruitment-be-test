using System.Collections.Generic;

namespace VacationRental.Business
{
    public class Rental : EntityBase
    {
        public int Units { get; set; }
        public RentalTypeEnum RentalType { get; set; }
        public List<Booking> BookingCollection { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
