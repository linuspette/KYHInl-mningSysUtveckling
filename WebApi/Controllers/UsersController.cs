using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ITokenHandler _tokenHandler;

        public UsersController(IUserManager userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync(int take = 0)
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (await _tokenHandler.ValidateTokenAsync(token) == ITokenHandler.StatusCode.Valid)
                return await _userManager.GetAllUsersAsync(take);

            return new UnauthorizedObjectResult("Session has expired");
        }
    }
}
