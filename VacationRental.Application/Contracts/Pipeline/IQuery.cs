using MediatR;

namespace VacationRental.Application.Contracts.Mediatr
{
    public interface IQuery<out TResponse> where TResponse : class
    {
    }
}
