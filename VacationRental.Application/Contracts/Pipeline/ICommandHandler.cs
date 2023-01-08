
namespace VacationRental.Application.Contracts.Pipeline
{
    public interface ICommandHandler<in TCommand, TResponse>  where TCommand : ICommand<TResponse>
    where TResponse : class
    {
        TResponse Handle(TCommand command);        
    }
}
