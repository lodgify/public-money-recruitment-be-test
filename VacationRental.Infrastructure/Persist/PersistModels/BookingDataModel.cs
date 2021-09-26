using System;

namespace VacationRental.Infrastructure.Persist.PersistModels
{
    public class BookingDataModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public TimePeriodDataModel Period { get; set; }
        public int PreparationInDays { get; set; }

        public int Unit { get; set; }
    }
}
