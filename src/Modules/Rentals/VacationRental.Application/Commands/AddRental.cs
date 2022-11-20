using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Application.Commands
{
    internal record AddRental(int Units, int PreparationTimeInDays) : ICommand<int>
    {
    }
}
