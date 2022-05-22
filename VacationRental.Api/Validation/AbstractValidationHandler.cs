using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation
{
    public abstract class AbstractValidationHandler : IValidationHandler
    {
        private IValidationHandler _nextHandler;

        public virtual void Handle(BookingBindingModel model, IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            if (this._nextHandler != null)
            {
                 this._nextHandler.Handle(model,rentals,bookings);
            }

        }

        public IValidationHandler SetNext(IValidationHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }
    }
}
