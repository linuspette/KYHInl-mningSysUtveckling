using ClassLibrary.Models.Input.Devices;
using ClassLibrary.Models.View.Device;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ClassLibrary.Models.Response.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace Device.IntelliFan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isRunnning = false;
        private bool _isConnected = false;
        private string _connectionState = "Connecting . . .";

        private DeviceClient _deviceClient;
        //UI Timer that checks every 5 seconds if application is connected to Iot-Hub
        private DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 5),
            IsEnabled = false,
            Tag = null
        };
        //Hard-coded device data
        private readonly DeviceData _device = new DeviceData
        {
            DeviceId = "IntelliFan-9071uy",
            DeviceType = "SmartFan",
            Location = "Living Room",
            Owner = "Linus Pettersson"
        };

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            _timer.Tick += new EventHandler(CheckConnectionStatus);
            _timer.Start();
            _isConnected = Task.Run(async () =>
            {
                while (!_isConnected)
                {
                    using var httpClient = new HttpClient();

                    var result = await httpClient.PostAsJsonAsync("https://sysdevfunctions.azurewebsites.net/api/devices", new AddDeviceRequest
                    {
                        DeviceId = _device.DeviceId,
                        DeviceType = _device.DeviceType,
                        Location = _device.Location,
                        Owner = _device.Owner,
                    });

                    if (result.IsSuccessStatusCode || result.StatusCode == HttpStatusCode.Conflict)
                    {
                        var data = JsonConvert.DeserializeObject<AddDeviceResponse>(await result.Content.ReadAsStringAsync());
                        _deviceClient = DeviceClient.CreateFromConnectionString(data.DeviceConnectionString);

                        var twin = await _deviceClient.GetTwinAsync();
                        if (twin != null)
                        {
                            TwinCollection reported = new TwinCollection();
                            reported["owner"] = _device.Owner;
                            reported["deviceType"] = _device.DeviceType;
                            reported["location"] = _device.Location;

                            await _deviceClient.UpdateReportedPropertiesAsync(reported);

                            return true;
                        }
                    }
                }
                return false;
            }).Result;
        }
        private void CheckConnectionStatus(object sender, EventArgs e)
        {
            if (!_isConnected)
                textBlockConnectionState.Text = "Not connected";
            else
                textBlockConnectionState.Text = "Connected";
        }
        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            var iconRotateFanBladesStoryboard = (BeginStoryboard)TryFindResource("iconRotateFanBladesStoryboard");

            if (_isRunnning)
            {
                _isRunnning = false;
                iconRotateFanBladesStoryboard.Storyboard.Stop();
                btnAction.Content = "Start";
            }
            else
            {
                _isRunnning = true;
                iconRotateFanBladesStoryboard.Storyboard.Begin();
                btnAction.Content = "Stop";
            }
        }
    }
}
