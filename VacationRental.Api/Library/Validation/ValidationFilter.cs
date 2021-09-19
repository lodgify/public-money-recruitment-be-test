using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Validation;

namespace VacationRental.Api
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                KeyValuePair<string, IEnumerable<string>>[] errors = context.ModelState.Where(state => state.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(error => error.ErrorMessage))
                    .ToArray();

                var errorResponse = new Errors();
                foreach (KeyValuePair<string, IEnumerable<string>> error in errors)
                {
                    foreach (string innerError in error.Value)
                    {
                        var errorModel = new ErrorsModel
                        {
                            Field = error.Key,
                            Message = innerError
                        };
                        errorResponse.ErrorsModels.Add(errorModel);
                    }
                }
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}
