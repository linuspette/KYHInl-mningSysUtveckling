using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ClassLibrary.Models.Input.Devices;
using ClassLibrary.Models.Response.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;

namespace Device.IntelliFan.Services;

public interface IDeviceService
{
    Task<bool> InitializeDeviceConnection(AddDeviceRequest deviceRequest, DeviceClient deviceClient);
}
public class DeviceService : IDeviceService
{
    public DeviceService()
    {

    }
    public async Task<bool> InitializeDeviceConnection(AddDeviceRequest deviceRequest, DeviceClient deviceClient)
    {
        using var httpClient = new HttpClient();

        var result = await httpClient.PostAsJsonAsync("https://sysdevfunctions.azurewebsites.net/api/devices", deviceRequest);

        if (result.IsSuccessStatusCode || result.StatusCode == HttpStatusCode.Conflict)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<AddDeviceResponse>(await result.Content.ReadAsStringAsync());
                deviceClient = DeviceClient.CreateFromConnectionString(data.DeviceConnectionString);

                var twin = await deviceClient.GetTwinAsync();
                if (twin != null)
                {
                    TwinCollection reported = new TwinCollection();
                    reported["owner"] = deviceRequest.Owner;
                    reported["deviceType"] = deviceRequest.DeviceType;
                    reported["location"] = deviceRequest.Location;

                    await deviceClient.UpdateReportedPropertiesAsync(reported);

                    return true;
                }
            }
            catch { }
        }

        return false;
    }
}