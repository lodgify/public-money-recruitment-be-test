using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Models.Dtos;
using VacationRental.Services.Interfaces;

namespace VacationRental.Api.Host.Controllers
{
    [Authorize,
     ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]"),
     ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// Guest Sign In 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous, 
         HttpGet("login-guest"),
         ProducesResponseType(typeof(AccessTokenDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<AccessTokenDto>> SignInGuestAsync()
        {
            var result = await _service.SignInGuestAsync();

            return Ok(result);
        }
    }
}
