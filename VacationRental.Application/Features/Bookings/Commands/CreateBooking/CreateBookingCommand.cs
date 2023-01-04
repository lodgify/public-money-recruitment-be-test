using System;
using VacationRental.Application.Contracts.Mediatr;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Features.Bookings.Commands.CreateBooking
{
    public sealed record CreateBookingCommand(int RentalId, DateTime Start, int Nights, int Units) : ICommand<ResourceId>
    {
        
    }
}
