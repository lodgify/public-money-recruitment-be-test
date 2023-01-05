
namespace VacationRental.Application.Contracts.Mediatr
{
    public interface ICommand<out TResponse> where TResponse : class
    {
    }
}
