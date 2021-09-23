using System;

namespace VacationRental.Infrastructure.Persist.PersistModels
{
    public class BookingDataModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
