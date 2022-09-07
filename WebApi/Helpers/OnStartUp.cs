using ClassLibrary.Models.Input.Roles;
using ClassLibrary.Models.Input.Users;
using WebApi.Repositories;

namespace WebApi.Helpers;

public class OnStartUp
{
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;
    private readonly ITokenHandler _tokenHandler;

    public OnStartUp(IUserManager userManager, IRoleManager roleManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenHandler = tokenHandler;
    }

    public async Task InitializeAsync()
    {
        await CheckRolesAsync();
        await CheckForRootAsync();
        await _tokenHandler.MakeAllTokensInvalid();
        await _tokenHandler.DeleteInactiveTokens();
    }

    //Checks if root-user exists
    private async Task CheckForRootAsync()
    {
        if (await _userManager.GetUserByUsernameAsync("root") == null)
        {
            await _userManager.AddUserAsync(new AddUser
            {
                Username = "root",
                Password = "Admin123!"
            });
        }
    }

    private async Task CheckRolesAsync()
    {
        var rolesToCheck = new string[]
        {
            "Admin",
            "Regular"
        };

        foreach (var role in rolesToCheck)
        {
            if (!await _roleManager.CheckIfRoleExistsAsync(role))
                await _roleManager.CreateRoleAsync(new CreateRole
                {
                    Name = role
                });
        }
    }
}