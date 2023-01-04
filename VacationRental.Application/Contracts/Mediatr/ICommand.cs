using MediatR;

namespace VacationRental.Application.Contracts.Mediatr
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
