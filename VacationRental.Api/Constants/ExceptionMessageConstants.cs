namespace VacationRental.Api.Constants
{
    public static class ExceptionMessageConstants
    {
        public const string DefaultValidationError = "Validation error:";
        public const string BookingBindingIdValidationError = DefaultValidationError + " booking id should not be zero or negaive";
        public const string RentalIdValidationError = DefaultValidationError + " rental id should not be zero or negaive";
        public const string NightsValidationError = DefaultValidationError + " nigths should not be zero or negaive";
        public const string RentalNotFound = "Rental not found";
        public const string BookingNotFound = "Booking not found";
    }
}
