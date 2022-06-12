using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Translator.Rental
{
    public static class Translator
    {
        public static RentalViewModel ViewModelTranslator(this RentalBindingModel model)
        {
            var rentalViewModel = new RentalViewModel
            {
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            return rentalViewModel;
        }
    }
}
