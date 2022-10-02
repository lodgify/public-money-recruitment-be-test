using System.Runtime.Serialization;

namespace VacationRental.Domain.VacationRental.Extensions.Enum
{
    public enum EnumExceptions
    {
        [EnumMember(Value = "No rental was found")]
        RentalNotFound = 1,

        [EnumMember(Value = "No booking was found")]
        BookingNotFound = 2,

        [EnumMember(Value = "Nights must be in minimum one.")]
        NightsConflict = 3,

        [EnumMember(Value = "Not available.")]
        AvailableConflict = 4,

        [EnumMember(Value = "There is a internal error on the aplication.")]
        InternalError = 5,
    }
}
