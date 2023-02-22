namespace VacationRental.Api.Constants
{
    public static class ExceptionMessageConstants
    {
        public const string DefaultValidationError = "Validation error:";
        public const string BookingBindingIdValidationError = DefaultValidationError + " booking id should not be negaive";
        public const string RentalIdValidationError = DefaultValidationError + " rental id should not be negaive";
        public const string UnitIdValidationError = DefaultValidationError + " unit id should not be negaive";
        public const string RentalUnitValidationError = DefaultValidationError + " units should not be less than previous value";
        public const string NightsValidationError = DefaultValidationError + " nigths should not be zero or negaive";
        public const string RentalNotFound = "Rental not found";
        public const string RentalNotAvailable = "Rental is not available";
        public const string BookingNotFound = "Booking not found";
        public const string PreparationTimeUpdateError = "Coudnt update preparation time";
        public const string ValidationUpdateError = DefaultValidationError + " nothing to update";
        public const string CreateUnitsValidationError = DefaultValidationError + " no free units to delete";
    }
}
