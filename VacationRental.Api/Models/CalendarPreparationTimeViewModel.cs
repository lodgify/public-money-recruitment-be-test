namespace VacationRental.Api.Models
{
    public class CalendarPreparationTimeViewModel
    {
        public CalendarPreparationTimeViewModel(int unit)
        {
            Unit = unit;
        }
        
        public int Unit { get; }
        
        public override int GetHashCode() => Unit;
    }
}
