using System.Threading;
using System.Threading.Tasks;
using VR.Application.Base;
using VR.DataAccess;
using VR.Domain.Models;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Requests.AddRental
{
    public class AddRentalRequestHandler : BaseRequestHandler<AddRentalRequest, AddRentalResponse>
    {
        public AddRentalRequestHandler(IObjectMapper mapper, VRContext context) : base(mapper, context) { }

        public async override Task<AddRentalResponse> Handle(AddRentalRequest request, CancellationToken cancellationToken)
        {
            var rental = new Rental
            {
                Units = request.Units,
                PreparationTimeInDays = request.PreparationTimeInDays,
            };

            await _context.Rentals.AddAsync(rental, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Rental, AddRentalResponse>(rental);
        }
    }
}