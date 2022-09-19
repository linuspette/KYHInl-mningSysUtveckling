using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Input.Users;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ITokenHandler _tokenHandler;

        public AuthenticationController(IUserManager userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignInAsync(SignIn model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("All required fields must be supplied and valid");

            return await _userManager.SignInAsync(model);
        }
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUpAsync(SignUp model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("All required fields must be supplied and valid");

            return await _userManager.AddUserAsync(model);
        }

        [HttpPost]
        [Route("validatetoken")]
        public async Task<IActionResult> CheckTokenValidityAsync(string token)
        {
            var result = await _tokenHandler.ValidateTokenAsync(token);

            if (result == ITokenHandler.StatusCode.Valid)
                return new OkObjectResult(true);

            return new UnauthorizedObjectResult(false);
        }
    }
}
