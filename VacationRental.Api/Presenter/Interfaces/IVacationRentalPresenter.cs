using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Presenter.Interfaces
{
    public interface IVacationRentalPresenter
    {
        IActionResult GetResult(EntityResult<ResourceIdViewModel> result);
        IActionResult GetResult(EntityResult<RentalViewModel> result);
        IActionResult GetResult(EntityResult<BookingViewModel> result);
        IActionResult GetResult(EntityResult<CalendarViewModel> result);
    }
}
