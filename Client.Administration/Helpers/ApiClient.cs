using Client.Administration.Models;
using Shared.Models.Input.Users;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Administration.Helpers;

public interface IApiClient
{
    public Task<bool> SignInAsync(SignIn model);
    public Task<HttpResponse> SignUpAsync(SignUp model);
    public Task<bool> ValidateTokenAsync();

}
public class ApiClient : IApiClient
{
    private readonly ITokenManager _tokenManager;
    private readonly HttpClient _httpClient;

    public ApiClient(ITokenManager tokenManager)
    {
        _tokenManager = tokenManager;
        _httpClient = new HttpClient();
    }

    //Authentication
    public async Task<bool> SignInAsync(SignIn model)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync<SignIn>($"{Properties.Settings.Default.ApiBaseAddress}authentication/signin", model);

            if (result.IsSuccessStatusCode)
            {
                _tokenManager.SetAccessToken($"bearer {await result.Content.ReadAsStringAsync()}");
                return true;
            }
        }
        catch { }

        return false;
    }
    public async Task<HttpResponse> SignUpAsync(SignUp model)
    {
        try
        {
            var result =
                await _httpClient.PostAsJsonAsync<SignUp>($"{Properties.Settings.Default.ApiBaseAddress}/authentication/signup", model);

            if (result.IsSuccessStatusCode)
            {
                return new HttpResponse
                {
                    Message = "Sign up OK",
                    Suceeded = true
                };
            }

            return new HttpResponse
            {
                Message = await result.Content.ReadAsStringAsync(),
                Suceeded = false
            };
        }
        catch { }

        return new HttpResponse
        {
            Message = "Error",
            Suceeded = false
        };
    }
    public async Task<bool> ValidateTokenAsync()
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync<string>($"{Properties.Settings.Default.ApiBaseAddress}/authentication/validatetoken", _tokenManager.GetAccessToken());
            if (result.IsSuccessStatusCode) { }
            return bool.Parse(await result.Content.ReadAsStringAsync());
        }
        catch { }

        return false;
    }

    //IotDevices
    //public async Task<List<IotDevice>> GetUserIotDevices()
    //{

    //}
}