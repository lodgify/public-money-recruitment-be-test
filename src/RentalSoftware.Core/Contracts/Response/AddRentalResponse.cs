using RentalSoftware.Core.Entities;

namespace RentalSoftware.Core.Contracts.Response
{
    public class AddRentalResponse : ResponseBase
    {
        public IdentifierViewModel ResourceIdViewModel { get; set; }
    }
}
