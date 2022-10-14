using LpSmartHub.MVVM.Models;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LpSmartHub.Services;

public interface IDeviceService
{
    public Task<List<DeviceItem>> GetDevicesAsync(string query);
    public Task<CloudToDeviceMethodResult> SendDirectMethodAsync(DirectMethodRequest directMethodRequest);
    public Task RemoveIotDeviceAsync(string deviceId);
    public string GetIotHubConnectionString();
}
public class DeviceService : IDeviceService
{
    private readonly string conString = "HostName=LinusDevIoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Si1Cocl5wG+fGHEYsiDEwoRBP1F1Gz7Q+Ap7mC8+tQY=";
    public async Task<List<DeviceItem>> GetDevicesAsync(string query)
    {
        var devices = new List<DeviceItem>();

        try
        {
            using var registryManager = RegistryManager.CreateFromConnectionString(conString);
            var result = registryManager.CreateQuery(query);

            if (result.HasMoreResults)
            {
                foreach (var twin in await result.GetNextAsTwinAsync())
                {
                    var device = new DeviceItem
                    {
                        DeviceId = twin.DeviceId
                    };

                    try { device.DeviceName = twin.Properties.Reported["deviceName"].ToString(); }
                    catch { }
                    try { device.DeviceType = twin.Properties.Reported["deviceType"].ToString(); }
                    catch { }
                    try { device.Location = twin.Properties.Reported["location"].ToString(); }
                    catch { }

                    try { device.Interval = twin.Properties.Reported["interval"]; }
                    catch { device.Interval = 10000; }

                    try { device.DeviceState = twin.Properties.Reported["deviceState"]; }
                    catch { }

                    switch (device.DeviceType.ToLower())
                    {
                        case "fan":
                            device.IconActive = "\ue004";
                            device.IconInActive = "\ue004";
                            break;
                        case "tempsensor":
                            device.IconActive = "\uf2c7";
                            device.IconInActive = "\uf2cb";
                            break;
                        default:
                            device.IconActive = "\uf2db";
                            device.IconInActive = "\uf2db";
                            break;
                    }

                    devices.Add(device);
                }
            }
        }
        catch { }

        return devices;
    }

    public async Task<CloudToDeviceMethodResult> SendDirectMethodAsync(DirectMethodRequest directMethodRequest)
    {
        try
        {
            using var serviceClient = ServiceClient.CreateFromConnectionString(conString);

            var cloudToDeviceMethod = new CloudToDeviceMethod(directMethodRequest.MethodName);
            if (directMethodRequest.Payload != null)
                cloudToDeviceMethod.SetPayloadJson(JsonConvert.SerializeObject(directMethodRequest.Payload));

            var result = await serviceClient.InvokeDeviceMethodAsync(directMethodRequest.DeviceId, cloudToDeviceMethod);
            return result;
        }
        catch { }
        return null!;
    }

    public async Task RemoveIotDeviceAsync(string deviceId)
    {
        try
        {
            using var registryManager = RegistryManager.CreateFromConnectionString(conString);
            await registryManager.RemoveDeviceAsync(deviceId);
        }
        catch { }
    }

    public string GetIotHubConnectionString()
    {
        return conString;
    }
}