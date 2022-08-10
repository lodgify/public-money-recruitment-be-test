using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Models;

namespace VacationRental.Api.Tests.Stubs
{
    public class RentalStubs
    {
        public static RentalViewModel SingleRental()
        {
            return new RentalViewModel()
            {
                Id = 1,
                PreparationTimeInDays = 2,
                Units = 3
            };
        }

        public static RentalBindingModel RentalBindingModel()
        {
            return new RentalBindingModel()
            {
                PreparationTimeInDays = 2,
                Units = 3
            };
        }
    }
}
