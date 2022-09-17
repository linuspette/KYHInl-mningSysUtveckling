using Shared.Models.Input.Devices;

namespace WebApi.Helpers;

public interface IAzureFunctionsClient
{
    public enum StatusCode
    {
        Succeded,
        Failed
    }

    public Task<IAzureFunctionsClient.StatusCode> AddIotDevice(AddDeviceRequest model);
}
public class AzureFunctionsClient : IAzureFunctionsClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _baseAdress = "https://sysdevfunctions.azurewebsites.net/api/devices/";

    public AzureFunctionsClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IAzureFunctionsClient.StatusCode> AddIotDevice(AddDeviceRequest model)
    {
        var response = await _httpClient.PostAsJsonAsync<AddDeviceRequest>("devices/add", model);

        if (response.IsSuccessStatusCode)
            return IAzureFunctionsClient.StatusCode.Succeded;

        return IAzureFunctionsClient.StatusCode.Failed;
    }
}