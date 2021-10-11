using System.Collections.Generic;

namespace VacationRental.Booking.Domain
{
    public class Calendar
    {
        public int RentalId { get; set; }
        public IList<CalendarDay> Dates { get; set; }

        public Calendar(int rentalId, IList<CalendarDay> dates)
        {
            this.Update(rentalId, dates);
        }

        public static Calendar Create(int rentalId, IList<CalendarDay> dates)
        {
            return new Calendar(rentalId, dates);
        }


        public void Update(int rentalId, IList<CalendarDay> dates)
        {
            this.Dates = dates;
            this.RentalId = rentalId;
        }
    }
}
