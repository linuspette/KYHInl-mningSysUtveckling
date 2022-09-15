using Shared.Models.Input.Roles;
using Shared.Models.Input.Users;
using WebApi.Repositories;

namespace WebApi.Helpers;

public class OnStartUp
{
    private readonly IUserManager _userManager;
    private readonly ITokenHandler _tokenHandler;

    public OnStartUp(IUserManager userManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
    }

    public async Task InitializeAsync()
    {
        await _tokenHandler.MakeAllTokensInvalid();
        await _tokenHandler.DeleteInactiveTokens();
    }
}