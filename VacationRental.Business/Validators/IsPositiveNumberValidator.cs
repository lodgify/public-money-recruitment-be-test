namespace VacationRental.Business.Validators
{
    public static class IsPositiveNumberValidator
    {
        public static bool Validate(int value)
        {
            return value > 0;
        }
    }
}
