using System.Threading;
using System.Threading.Tasks;
using VacationRental.Shared.Abstractions.Commands;
using VacationRental.Shared.Abstractions.Dispatchers;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Shared.Infrastructure.Dispatchers
{
    internal class InMemoryDispatcher : IDispatcher
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public InMemoryDispatcher(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
            => _commandDispatcher.SendAsync(command, cancellationToken);

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            => _queryDispatcher.QueryAsync(query, cancellationToken);
    }
}
