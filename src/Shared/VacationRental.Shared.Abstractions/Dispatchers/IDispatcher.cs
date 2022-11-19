using System.Threading;
using System.Threading.Tasks;
using VacationRental.Shared.Abstractions.Commands;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Shared.Abstractions.Dispatchers
{
    public interface IDispatcher
    {
        Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
