using MediatR;
using System;
using VacationRental.Api.Models;
using VacationRental.Core;

namespace VacationRental.Api.Features
{
    public class GetRental
    {
        public class Query : IRequest<RentalViewModel>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : RequestHandler<Query, RentalViewModel>
        {
            private readonly IRentalRepository _rentals;

            public QueryHandler(IRentalRepository rentals)
            {
                _rentals = rentals;
            }

            protected override RentalViewModel Handle(Query request)
            {
                Rental rental = _rentals.Get(request.Id);

                if (rental is null)
                    throw new ApplicationException("Rental not found");

                return new RentalViewModel
                {
                    Id = rental.Id,
                    Units = rental.Units,
                    PreparationTimeInDays = rental.PreparationTimeInDays
                };
            }
        }
    }
}
