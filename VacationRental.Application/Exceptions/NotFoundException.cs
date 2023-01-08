using VacationRental.Domain.Primitives;

namespace VacationRental.Application.Exceptions
{
    public sealed class NotFoundException : BaseException
    {
        public NotFoundException(DomainError error) : base(error)
        {

        }
    }
}
