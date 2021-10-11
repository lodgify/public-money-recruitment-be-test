using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel : BookingBindingModel
    {
        public int Id { get; set; }
        public int Unit { get; set; }
    }
}
