using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.ViewModel;

namespace VacationRental.Application.Rentals.Queries.GetRental
{
    public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, RentalViewModel>
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public GetRentalQueryHandler(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        public async Task<RentalViewModel> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            if (!_rentals.ContainsKey(request.RentalId))
                return null;
    
            return await Task.FromResult(_rentals[request.RentalId]);
        }
    }
}