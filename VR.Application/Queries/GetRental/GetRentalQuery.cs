using MediatR;

namespace VR.Application.Queries.GetRental
{
    public class GetRentalQuery : IRequest<GetRentalResponse>
    {
        public GetRentalQuery(int rentalId)
        {
            RentalId = rentalId;
        }

        public int RentalId { get; }
    }
}
