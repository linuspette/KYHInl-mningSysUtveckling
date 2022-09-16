using System;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Administration.Models;
using Microsoft.Extensions.Configuration;
using Shared.Models.Input.Users;

namespace Client.Administration.Helpers;

public interface IApiClient
{
    public Task<bool> SignInAsync(SignIn model);
    public Task<HttpResponse> SignUpAsync(SignUp model);

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

    public async Task<bool> SignInAsync(SignIn model)
    {
        var result = await _httpClient.PostAsJsonAsync<SignIn>($"{Properties.Settings.Default.ApiBaseAddress}authentication/signin", model);

        if (result.IsSuccessStatusCode)
        {
            _tokenManager.SetAccessToken($"bearer {await result.Content.ReadAsStringAsync()}");
            return true;
        }

        return false;
    }

    public async Task<HttpResponse> SignUpAsync(SignUp model)
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
}