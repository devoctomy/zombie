using Microsoft.AspNetCore.Mvc;
using Zombie.Api.Mediator;

namespace Zombie.Api.Controller
{
    public class BaseController : ControllerBase
    {
        protected IActionResult ProcessResponse<T>(BaseResponse<T> response)
        {
            if(response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return StatusCode((int)response.StatusCode);
        }
    }
}
