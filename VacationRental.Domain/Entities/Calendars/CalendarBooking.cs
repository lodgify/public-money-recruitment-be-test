using VacationRental.Domain.Primitives;

namespace VacationRental.Api.Models
{
    public class CalendarBooking : BaseDomainModel
    {        
        public int Unit { get; private set; }

        private CalendarBooking(int id, int unit) : base()
        {
            Id = id;
            Unit = unit;
        }

        public static CalendarBooking Create(int id, int unit)
        {
            return new CalendarBooking(id, unit);
        }
    }

    
}
