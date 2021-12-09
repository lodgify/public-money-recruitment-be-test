using RentalSoftware.Core.Entities;

namespace RentalSoftware.Core.Contracts.Response
{
    public class GetRentalResponse : ResponseBase
    {
        public RentalViewModel RentalViewModel { get; set; }
    }
}
