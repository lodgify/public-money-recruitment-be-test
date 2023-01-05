using MediatR;

namespace VacationRental.Application.Contracts.Mediatr
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    where TResponse : class
    {
        TResponse Handle(TQuery command);
    }
}
