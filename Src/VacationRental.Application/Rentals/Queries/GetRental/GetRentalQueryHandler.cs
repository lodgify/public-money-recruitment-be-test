using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Rentals.Queries.GetRental
{
    public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, RentalViewModel>
    {
        private readonly IRentalRepository _repository;
        public GetRentalQueryHandler(IRentalRepository repository)
        {
            _repository = repository;
        }

        public async Task<RentalViewModel> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            var rental = _repository.Get(request.RentalId);

            if (rental == null)
                return null;
    
            return await Task.FromResult(new RentalViewModel()
            {
                Id = rental.Id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            });
        }
    }
}