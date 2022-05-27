using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Notifications;

namespace VacationRental.Api.Presenter.Interfaces
{
    public interface IBasePresenter
    {
        IActionResult GetActionResult<T, Y>(T result)
           where T : EntityResult<Y>
           where Y : class;

        IActionResult GetActionResult<T>(T result) where T : Result;
    }
}
