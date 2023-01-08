
namespace VacationRental.Application.Contracts.Pipeline
{
    public interface ICommand<out TResponse> where TResponse : class
    {
    }
}
