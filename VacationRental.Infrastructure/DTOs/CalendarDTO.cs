using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Infrastructure.DTOs
{
    public class CalendarDTO
    {
        public int RentalId { get; set; }

        public List<CalendarDateDTO> Dates { get; set; }
    }
}
