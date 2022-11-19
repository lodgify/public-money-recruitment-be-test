using System.Threading;
using System.Threading.Tasks;

namespace VacationRental.Shared.Abstractions.Commands
{
    public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
