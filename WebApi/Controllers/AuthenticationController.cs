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

        public AuthenticationController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("All required fields must be supplied and valid");

            return await _userManager.SignInAsync(model);
        }
    }
}
