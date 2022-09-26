using System;
using System.Diagnostics.CodeAnalysis;

namespace VacationRental.BusinessLogic.Services.Models
{
    [ExcludeFromCodeCoverage]
    public class GetCalendarServiceModel
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
