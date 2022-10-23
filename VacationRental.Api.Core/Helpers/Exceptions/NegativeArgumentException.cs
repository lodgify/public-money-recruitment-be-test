using System;

namespace VacationRental.Api.Core.Helpers.Exceptions
{
    public class NegativeArgumentException : Exception
    {
        public string Parameter { get; set; }
        public NegativeArgumentException() { }
        public NegativeArgumentException(string message) : base(message) { }
        public NegativeArgumentException(string message, string parameter) : this(message) 
        {
            Parameter = parameter;
        }
    }
}
