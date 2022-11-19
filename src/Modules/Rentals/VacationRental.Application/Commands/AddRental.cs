using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Application.Commands
{
    internal record AddRental(int Units) : ICommand<int>
    {
    }
}
