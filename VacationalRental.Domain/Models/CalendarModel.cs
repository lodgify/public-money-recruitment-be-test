using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationalRental.Domain.Models
{
    public class CalendarModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateModel> Dates { get; set; }
    }
}
