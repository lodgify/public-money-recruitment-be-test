using System;
using System.Linq;
using VacationRental.Api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Error = VacationRental.Api.ApplicationErrors.ErrorMessages;

namespace VacationRental.Api.Filters.Rentals
{
    public class RentalPropertyChangedFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _key;
        public RentalPropertyChangedFilterAttribute(string key)
        {
            _key = key;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int rentalId = (int)context.ActionArguments[_key];
            var model = context.ActionArguments["model"] as RentalBindingModel;
            var rentals = context.HttpContext.RequestServices.GetService<IDictionary<int, RentalViewModel>>();
            int currentUnits = rentals[rentalId].Units;
            int difference = currentUnits - model.Units;

            int currentPreparationTimeInDays = rentals[rentalId].PreparationTimeInDays;
            int preparationTimeInDays = model.PreparationTimeInDays;

            if (difference <= 0 && currentPreparationTimeInDays == preparationTimeInDays)
                return;
            else 
            {
                var bookings = context.HttpContext.RequestServices.GetService<IDictionary<int, BookingViewModel>>();
                int currentBookingsCount = bookings.Count;
                if (model.Units >= currentBookingsCount && currentPreparationTimeInDays == preparationTimeInDays)
                    return;
                else 
                {
                    int[] arrivalsDays = bookings.Values
                        .Select(x => x.Start.DayOfYear)
                        .ToArray();
                    int[] departuresDays = bookings.Values
                        .Select(x => x.Start.AddDays(x.Nights).DayOfYear + preparationTimeInDays)
                        .ToArray();

                    int[] days = new int[365];

                    int inc = 0;
                    for (int i = 0; i < days.Length; i++)
                    {
                        for (int j = 0; j < arrivalsDays.Length; j++ ) 
                        {
                            if (arrivalsDays[j] == i)
                            {
                                inc += 1;
                            }
                            days[i] = inc;
                        }
                        for (int k = 0; k < departuresDays.Length; k++)
                        {
                            if (departuresDays[k] == i)
                            {
                                inc -= 1;
                            }
                            days[i] = inc;
                        }
                    }
                    int maxDayDensity = days.Max();
                    if (maxDayDensity >= model.Units)
                        throw new ApplicationException(Error.UnallowedRentalEditing);
                }
            }
        }
    }
}
