using Api.Application.Common.BaseResponse;
using Api.Application.Interfaces.Collaborators;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphUserController : ControllerBase
    {
        private readonly IGraphUserService _userService;

        public GraphUserController(IGraphUserService userService) => _userService = userService;


        [HttpGet("logged-user")]
        public async Task<IActionResult> GetLoggedUser()
        {
            var oli = await _userService.Current();
            return Ok(BaseResponse.Ok(oli));
        }
    }
}
