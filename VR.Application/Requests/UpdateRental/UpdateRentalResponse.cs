using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VR.Application.Requests.UpdateRental
{
    public class UpdateRentalResponse
    {
        public int Id { get; set; }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
