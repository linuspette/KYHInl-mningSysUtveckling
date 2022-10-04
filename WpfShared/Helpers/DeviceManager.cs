using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Shared.Models.Input.Devices;
using Shared.Models.Response.Devices;
using System.Net.Http;
using System.Net.Http.Json;
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

    private static DeviceClient deviceClient = null!;
    private static string baseUrl = "https://sysdevfunctions.azurewebsites.net/api/devices/connect";
    private static string AzureFunctionsAccessToken = string.Empty;

    public static bool isConnected { get; private set; } = false;
    public static string ConnectionStateMessage { get; private set; } = null!;
    public static string DeviceId { get; private set; } = null!;
    public static string Owner { get; private set; } = null!;
    public static string DeviceType { get; private set; } = null!;

    public static void Initialize(string deviceId, string deviceType, string owner, string azureFunctionsAccessToken)
    {
        DeviceId = deviceId;
        DeviceType = deviceType;
        Owner = owner;
        AzureFunctionsAccessToken = azureFunctionsAccessToken;
    }

    public static void SetConnectionState(ConnectionState state)
    {
        switch (state)
        {
            case ConnectionState.NotConnected:
                ConnectionStateMessage = "Not connected";
                break;

            case ConnectionState.Connecting:
                ConnectionStateMessage = "Connecting. Please wait...";
                break;

            case ConnectionState.StillConnecting:
                ConnectionStateMessage = "Still connecting. Please wait...";
                break;

            case ConnectionState.Initializing:
                ConnectionStateMessage = "Initializing. Please wait...";
                break;

            case ConnectionState.Connected:
                isConnected = true;
                ConnectionStateMessage = "Connected";
                break;
        }
    }

    public static async Task ConnectAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!isConnected)
            {
                SetConnectionState(i > 5 ? ConnectionState.StillConnecting : ConnectionState.Connecting);

                try
                {
                    using (var _httpClient = new HttpClient())
                    {
                        var response =
                            await _httpClient.PostAsJsonAsync(baseUrl, new HttpDeviceRequest { DeviceId = DeviceId });
                        if (response.IsSuccessStatusCode)
                        {
                            var data = JsonConvert.DeserializeObject<HttpDeviceResponse>(
                                await response.Content.ReadAsStringAsync());
                            if (data != null)
                            {
                                try
                                {
                                    deviceClient = DeviceClient.CreateFromConnectionString(data.ConnectionString);
                                    SetConnectionState(ConnectionState.Initializing);
                                    var twinCollection = new TwinCollection();
                                    twinCollection["owner"] = Owner;
                                    twinCollection["deviceType"] = DeviceType;

                                    await deviceClient.UpdateReportedPropertiesAsync(twinCollection);
                                    var twin = deviceClient.GetTwinAsync();

                                    var result = await _httpClient.GetAsync($"{baseUrl}?code={AzureFunctionsAccessToken}&deviceId={DeviceId}");
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
        await deviceClient.SendEventAsync(msg);
    }
}