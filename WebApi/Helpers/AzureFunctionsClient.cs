namespace WebApi.Helpers;

public interface IAzureFunctionsClient
{

}
public class AzureFunctionsClient : IAzureFunctionsClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient = new HttpClient();
    public AzureFunctionsClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["SysDevWebApiConnection"]);
    }
}