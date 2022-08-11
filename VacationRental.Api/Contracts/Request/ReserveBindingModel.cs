using System;

namespace VacationRental.Api.Contracts.Request
{
    public class ReserveBindingModel
    {
        public int RentalId { get; set; }
        
        public DateTime Start { get; set; }
        
        public int Nights { get; set; }
    }
}