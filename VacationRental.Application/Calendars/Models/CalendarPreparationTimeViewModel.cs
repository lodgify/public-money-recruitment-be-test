using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Application.Calendars.Models
{
    public class CalendarPreparationTimeViewModel
    {
        public CalendarPreparationTimeViewModel(int unit)
        {
            Unit = unit;
        }

        public int Unit { get; set; }
    }
}
