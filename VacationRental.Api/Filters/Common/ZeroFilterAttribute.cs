using Microsoft.AspNetCore.Mvc.Filters;
using System;


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
                int rentalId = (int)context.ActionArguments[key];
                if (rentalId <= 0)
                    throw new ApplicationException($"{key} couldn't be equal or less zero");
            }
        }
    }
}
