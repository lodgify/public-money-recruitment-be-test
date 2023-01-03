using System;
using VacationRental.Domain.Primitives;

namespace VacationRental.Api.Models
{
    public class Booking : BaseDomainModel
    {        
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
