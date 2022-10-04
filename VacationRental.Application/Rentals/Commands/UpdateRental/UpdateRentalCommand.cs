using MediatR;

namespace VacationRental.Application.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int RentalId { get; private set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        public void SetRentalId(int rentalId)
        {
            RentalId = rentalId;
        }
    }
}
