using MediatR;

namespace VacationRental.Application.Contracts.Mediatr
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
     where TQuery : IQuery<TResponse>
    {
    }
}
