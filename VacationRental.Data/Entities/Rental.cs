using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Data.Entities
{
    public class Rental : BaseEntity
    {
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
