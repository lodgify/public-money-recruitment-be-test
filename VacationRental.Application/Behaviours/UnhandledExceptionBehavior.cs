using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VacationRental.Application.Behaviours
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {        

        public UnhandledExceptionBehavior()
        {            
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;                
                throw;
            }
        }
    }

}
