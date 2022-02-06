using System;

namespace Application.Models.Calendar.Requests
{
    public class GetCalendarRequest
    {
       public int RentalId { get; set; }
       public DateTime Start { get; set; }
       public int Nights { get; set; }
    }
}