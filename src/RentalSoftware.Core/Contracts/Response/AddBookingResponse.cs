using RentalSoftware.Core.Entities;

namespace RentalSoftware.Core.Contracts.Response
{
    public class AddBookingResponse : ResponseBase
    {
        public IdentifierViewModel ResourceIdViewModel { get; set; }
    }
}
