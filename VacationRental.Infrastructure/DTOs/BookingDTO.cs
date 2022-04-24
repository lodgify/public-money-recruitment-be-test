using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Infrastructure.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }

        public int RentalId { get; set; }

        public int Unit { get; set; }

        public DateTime Start { get; set; }

        public int Nights { get; set; }
    }
}
