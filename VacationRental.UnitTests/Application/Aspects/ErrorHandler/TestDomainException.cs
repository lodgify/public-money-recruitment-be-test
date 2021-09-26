using VacationRental.Domain.Exceptions;

namespace VacationRental.UnitTests.Application.Aspects.ErrorHandler
{
    public class TestDomainException : DomainException
    {
        public const string ErrorMessage = "Test Error Message";
        public TestDomainException() : base(ErrorMessage)
        {

        }
    }
}
