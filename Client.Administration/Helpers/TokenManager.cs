using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Client.Administration.Helpers;

public interface ITokenManager
{
    public string GetAccessToken();
    public void SetAccessToken(string accessToken);
}
public class TokenManager : ITokenManager
{
    public string GetAccessToken()
    {
        return Properties.Settings.Default.AccessToken;
    }

    public void SetAccessToken(string accessToken)
    {
        Properties.Settings.Default.AccessToken = accessToken;
    }
}