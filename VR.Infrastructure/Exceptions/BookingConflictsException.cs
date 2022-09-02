using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VR.Infrastructure.Exceptions
{
    public class BookingConflictsException : VacationRentalException
    {
        public BookingConflictsException()
            : base("BookingConflicts", null)
        {
        }

        public BookingConflictsException(string title)
            : base(title, null)
        {
        }

        public BookingConflictsException(string title, string details)
            : base(title, details)
        {
        }
    }
}
