using MediatR;

namespace VR.Application.Requests.UpdateRental
{
    public class UpdateRentalRequest : IRequest<UpdateRentalResponse>
    {
        public int Id { get; set; }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
