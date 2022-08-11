using System.Text.Json.Serialization;

namespace VacationRental.Api.Contracts.Request
{
    public class UpdateRentalBindingModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
