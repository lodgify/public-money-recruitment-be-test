using System;
using System.Net;

namespace VacationRental.Domain.Models
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
