namespace VacationRental.Application.Contracts.Pipeline
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    where TResponse : class
    {
        TResponse Handle(TQuery command);
    }
}
