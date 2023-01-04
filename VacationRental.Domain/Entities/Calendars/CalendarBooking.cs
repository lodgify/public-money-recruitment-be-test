using VacationRental.Domain.Primitives;

namespace VacationRental.Api.Models
{
    public class CalendarBooking : BaseDomainModel
    {                
        public int Unit { get; private set; }

        private CalendarBooking(int unit) : base()
        {            
            Unit = unit;
        }

        public static CalendarBooking Create(int unit)
        {
            return new CalendarBooking(unit);
        }
    }

    
}
