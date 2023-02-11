﻿using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Domain.Messages.Rentals;

namespace VacationRental.Application.Features.Rentals.Queries.GetRental
{
    public sealed record class GetRentalQuery(int RentalId) :IQuery<RentalDto>
    {
    }
}
