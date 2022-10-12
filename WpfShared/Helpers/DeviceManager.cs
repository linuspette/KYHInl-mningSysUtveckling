using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Shared.Models.Input.Devices;
using Shared.Models.Iot;
using Shared.Models.Response.Devices;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WpfShared.Helpers;

public static class DeviceManager
{
    public enum ConnectionState
    {
        NotConnected,
        Connecting,
        StillConnecting,
        Initializing,
        Connected
    }

    public static DeviceSettings _deviceSettings;
    private static DeviceClient deviceClient = null!;
    private static string baseUrl = "https://sysdevfunctions.azurewebsites.net/api/devices/connect";

    public static bool isConnected { get; set; } = false;
    public static string ConnectionStateMessage { get; set; } = null!;

    public static void Initialize(DeviceSettings deviceSettings)
    {
        _deviceSettings = deviceSettings;
    }

    public static void SetConnectionState(ConnectionState state)
    {
        switch (state)
        {
            case ConnectionState.NotConnected:
                DeviceManager.ConnectionStateMessage = "Not connected";
                break;

            case ConnectionState.Connecting:
                DeviceManager.ConnectionStateMessage = "Connecting. Please wait...";
                break;

            case ConnectionState.StillConnecting:
                DeviceManager.ConnectionStateMessage = "Still connecting. Please wait...";
                break;

            case ConnectionState.Initializing:
                DeviceManager.ConnectionStateMessage = "Initializing. Please wait...";
                break;

            case ConnectionState.Connected:
                DeviceManager.isConnected = true;
                DeviceManager.ConnectionStateMessage = "Connected";
                break;
        }
    }

    public static async Task ConnectAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            if (isConnected)
            {
                SetConnectionState(i > 5 ? ConnectionState.StillConnecting : ConnectionState.Connecting);

                try
                {
                    using (var _httpClient = new HttpClient())
                    {
                        var response =
                            await _httpClient.PostAsJsonAsync(baseUrl, new HttpDeviceRequest { DeviceId = _deviceSettings.DeviceId });
                        if (response.IsSuccessStatusCode)
                        {
                            var data = JsonConvert.DeserializeObject<HttpDeviceResponse>(
                                await response.Content.ReadAsStringAsync());
                            if (data != null)
                            {
                                try
                                {
                                    deviceClient = DeviceClient.CreateFromConnectionString(data.ConnectionString, TransportType.Mqtt);
                                    SetConnectionState(ConnectionState.Initializing);
                                    var twin = await deviceClient.GetTwinAsync();

                                    try
                                    {
                                        _deviceSettings.Interval = (int)twin.Properties.Desired["interval"];
                                    }
                                    catch { }

                                    await SetDeviceTwinAsync();

                                    var result = await _httpClient.GetAsync($"{baseUrl}?deviceId={_deviceSettings.DeviceId}");
                                    if (result.IsSuccessStatusCode)
                                    {
                                        var connectionState = await result.Content.ReadAsStringAsync();
                                        if (connectionState == "Connected")
                                        {
                                            SetConnectionState(ConnectionState.Connected);
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }

                        await Task.Delay(1000);
                    }
                }
                catch { }
            }
            else
                SetConnectionState(ConnectionState.Connected);
        }
        if (!isConnected)
            SetConnectionState(ConnectionState.NotConnected);
    }

    public static async Task SendMessageToIotHubAsync(Message msg)
    {
        if (isConnected && deviceClient != null && _deviceSettings.DeviceState)
            await deviceClient.SendEventAsync(msg);
    }

    public static async Task SetDirectMethodAsync()
    {
        while (deviceClient == null)
        {
            if (deviceClient != null)
                await deviceClient.SetMethodHandlerAsync("OnOff", OnOff, null);
        }
    }

    private static Task<MethodResponse> OnOff(MethodRequest methodrequest, object usercontext)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<dynamic>(methodrequest.DataAsJson);
            _deviceSettings.DeviceState = data!.deviceState;

            SetDeviceTwinAsync().ConfigureAwait(false);
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_deviceSettings)), 200));

        }
        catch (Exception e)
        {
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)), 400));
        }
    }

    private static async Task SetDeviceTwinAsync()
    {
        var twinCollection = new TwinCollection();
        twinCollection["location"] = _deviceSettings.Location;
        twinCollection["owner"] = _deviceSettings.Owner;
        twinCollection["deviceType"] = _deviceSettings.DeviceType;
        twinCollection["deviceState"] = _deviceSettings.DeviceState;
        twinCollection["deviceName"] = _deviceSettings.DeviceName;

        await deviceClient.UpdateReportedPropertiesAsync(twinCollection);
    }
}