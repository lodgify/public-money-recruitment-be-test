using System;
using System.Runtime.Serialization;

namespace VR.Infrastructure.Exceptions
{
    public class NotFoundException : VacationRentalException
    {
        public NotFoundException()
            : base("NotFound", null)
        {
        }

        public NotFoundException(string title)
            : base(title, null)
        {
        }

        public NotFoundException(string title, string details)
            : base(title, details)
        {
        }
    }
}
