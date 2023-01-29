using System;

namespace VacationRental.Application.ViewModels
{
    public class BookingViewModelOutput
    {
        public BookingViewModelOutput(int id, int rentalId, DateTime start, int nights)
        {
            this.Id = id;
            this.RentalId = rentalId;
            this.Start = start;
            this.Nights = nights;
        }

        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
