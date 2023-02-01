using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
        
        public override int GetHashCode() => Id;

        public bool IsOverlapping(DateTime start, int nights, int preparationTime)
        {
            return (Start <= start.Date && Start.AddDays(Nights + preparationTime) > Start.Date)
                   || (Start < start.AddDays(nights + preparationTime) && Start.AddDays(Nights + preparationTime) >=
                       Start.AddDays(nights + preparationTime))
                   || (Start > start &&
                       Start.AddDays(Nights + preparationTime) < start.AddDays(nights + preparationTime));
        }
    }
}
