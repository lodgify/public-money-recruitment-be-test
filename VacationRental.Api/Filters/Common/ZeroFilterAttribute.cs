using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;

namespace VacationRental.Api.Filters.Common
{
    public class ZeroFilterAttribute : Attribute, IActionFilter
    {
        private readonly string[] _keys;

        public ZeroFilterAttribute(params string[] keys)
        {
            _keys = keys;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (string key in _keys) 
            {
                int? rentalId = (int?)context.ActionArguments[key];
                if(rentalId == null)
                    throw new ApplicationException(Error.ValueCannotBeNull);
                if (rentalId <= 0)
                    throw new ApplicationException(Error.ValueCannotBeNull);
            }
        }
    }
}
