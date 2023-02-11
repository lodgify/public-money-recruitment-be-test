using FluentValidation.Results;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Application.Utils
{
    public static class ValidationUtils
    {
        public static string BuildErrors(List<ValidationFailure> errors)
        {
            var builder = new StringBuilder();
            foreach (var error in errors)
            {
                builder.Append(error.ErrorMessage);
            }

            return builder.ToString();
        }
    }
}
