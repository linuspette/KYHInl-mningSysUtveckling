using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
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
        private readonly ITokenReturnStatements _tokenReturnStatements;

        public UsersController(IUserManager userManager, ITokenHandler tokenHandler, ITokenReturnStatements tokenReturnStatements)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _tokenReturnStatements = tokenReturnStatements;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync(int take = 0)
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var result = await _tokenHandler.ValidateTokenAsync(token);
            if (result == ITokenHandler.StatusCode.Valid)
                return await _userManager.GetAllUsersAsync(take);
            else if (result == ITokenHandler.StatusCode.Expired)
                return new UnauthorizedObjectResult(_tokenReturnStatements.TokenReturnStatement(result));

            return new UnauthorizedObjectResult(_tokenReturnStatements.TokenReturnStatement(result));
        }
    }
}
