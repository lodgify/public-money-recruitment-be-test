
namespace RentalSoftware.Core.Contracts.Request
{
    public class UpdateRentalRequest
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
