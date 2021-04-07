using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationalRental.Domain.Models
{
    public class VacationalRentalModel
    {
        public int RentalId { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
        public int UnitsBooked { get; set; }
    }
}
