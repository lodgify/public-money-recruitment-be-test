using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Features.Rentals.Commands.CreateRental
{
    public sealed record class CreateRentalCommand(int Units, int PreparationTimeInDays) : ICommand<ResourceId>
    {
    }
}
