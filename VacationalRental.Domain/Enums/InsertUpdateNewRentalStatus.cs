using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationalRental.Domain.Enums
{
    public enum InsertUpdateNewRentalStatus
    {
        InsertUpdateDbNoRowsAffected = 1,
        OK,
        RentalNotExists,
        UnitsQuantityBookedAlready,
        DatesOverlapping
    }
}
