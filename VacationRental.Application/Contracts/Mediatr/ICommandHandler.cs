using MediatR;

namespace VacationRental.Application.Contracts.Mediatr
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    {
    }
}
