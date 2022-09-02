using System;
using System.Runtime.Serialization;

namespace VR.Infrastructure.Exceptions
{
    public class VacationRentalException : ApplicationException
    {
        public VacationRentalException()
        {
        }

        public VacationRentalException(string title, string details)
            : base(title)
        {
            Details = details;
        }

        public VacationRentalException(string title, string details, Exception innerException)
            : base(title, innerException)
        {
            Details = details;
        }

        public string Title => base.Message;

        public string Details { get; }
    }
}
