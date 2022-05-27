using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Presenter.Interfaces;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Presenter
{
    public class VacationRentalPresenter:BasePresenter,IVacationRentalPresenter
    {
        public IActionResult GetResult(EntityResult<ResourceIdViewModel> result)
        {
            return result.Invalid ? base.GetActionResult(result) :
                new OkObjectResult(result.Entity);
        }

        public IActionResult GetResult(EntityResult<RentalViewModel> rentalViewModel)
        {
            return rentalViewModel.Invalid ? base.GetActionResult(rentalViewModel) :
                new OkObjectResult(rentalViewModel.Entity);
        }
        public IActionResult GetResult(EntityResult<BookingViewModel> bookingViewModel)
        {
            return bookingViewModel.Invalid ? base.GetActionResult(bookingViewModel) :
                new OkObjectResult(bookingViewModel.Entity);
        }

        public IActionResult GetResult(EntityResult<CalendarViewModel> calendarViewModel)
        {
            return calendarViewModel.Invalid ? base.GetActionResult(calendarViewModel) :
                new OkObjectResult(calendarViewModel.Entity);
        }
    }
}
