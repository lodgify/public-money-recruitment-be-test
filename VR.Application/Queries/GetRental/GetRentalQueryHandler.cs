using System.Threading;
using System.Threading.Tasks;
using VR.Application.Base;
using VR.DataAccess;
using VR.Domain.Models;
using VR.Infrastructure.Exceptions;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Queries.GetRental
{
    public class GetRentalQueryHandler : BaseRequestHandler<GetRentalQuery, GetRentalResponse>
    {
        public GetRentalQueryHandler(IObjectMapper mapper, VRContext context) : base(mapper, context) { }

        public async override Task<GetRentalResponse> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            var rental = await _context.Rentals.FindAsync(request.RentalId);

            if (rental == null)
            {
                throw new NotFoundException("Rental is not found",$"GetRentalQuery - rental with id {request.RentalId} not found");
            }

            return _mapper.Map<Rental, GetRentalResponse>(rental);
        }
    }
}
