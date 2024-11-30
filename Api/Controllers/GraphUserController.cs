using Api.Application.Common.BaseResponse;
using Api.Application.Interfaces.Collaborators;
using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(SyncCollaboratorFilter))]
    public class GraphUserController : ControllerBase
    {
        private readonly IGraphUserService _userService;

        public GraphUserController(IGraphUserService userService) => _userService = userService;

        [HttpGet("logged-user")]
        [SwaggerOperation(
            Summary = "Gets the currently logged-in user's information from Graph",
            Description = "Fetches details about the currently logged-in user, including basic information and roles."
        )]
        public async Task<IActionResult> GetLoggedUser()
        {
            var loggedUser = await _userService.Current();
            return Ok(BaseResponse.Ok(loggedUser));
        }

        [HttpGet("{userOid}")]
        [SwaggerOperation(
            Summary = "Gets a user's information and their manager from Graph",
            Description = "Fetches details about a user and their direct manager based on the user's Object ID (userOid)."
        )]
        public async Task<IActionResult> GetUserManager([FromRoute] string userOid)
        {
            var userManager = await _userService.FindUserWithManagerAsync(userOid);
            return Ok(BaseResponse.Ok(userManager));
        }
    }
}
