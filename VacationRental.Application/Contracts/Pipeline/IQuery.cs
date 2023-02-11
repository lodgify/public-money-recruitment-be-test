namespace VacationRental.Application.Contracts.Pipeline
{
    public interface IQuery<out TResponse> where TResponse : class
    {
    }
}
