using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationalRental.Domain.Enums
{
    public enum InsertNewBookingStatus
    {
        NotAvailable = 1,
        InsertDbNoRowsAffected,
        OK
    }
}
