using MediatR;

namespace VR.Application.Requests.AddRental
{
    public class AddRentalRequest : IRequest<AddRentalResponse>
    {
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
