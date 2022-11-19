using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Shared.Infrastructure.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            if (command is null)
            {
                throw new ArgumentNullException();
            }

            using var scope = _serviceProvider.CreateScope();
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync));
            if (method is null)
            {
                throw new InvalidOperationException($"Command handler for '{typeof(TResult).Name}' is invalid.");
            }

            return await (Task<TResult>)method.Invoke(handler, new object[] { command, cancellationToken });
        }
    }
}
