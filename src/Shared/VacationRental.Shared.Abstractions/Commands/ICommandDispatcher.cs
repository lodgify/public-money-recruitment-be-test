using System.Threading;
using System.Threading.Tasks;

namespace VacationRental.Shared.Abstractions.Commands
{
    public interface ICommandDispatcher
    {
        Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    }
}
