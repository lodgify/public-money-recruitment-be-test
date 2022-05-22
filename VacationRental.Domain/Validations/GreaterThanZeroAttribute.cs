using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Validations
{
    public class GreaterThanZeroAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int number = (int)value;
            return number > 0;
        }
    }
}
