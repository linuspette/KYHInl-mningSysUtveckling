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
using Device.IntelliFan.Services;
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
        private readonly IDeviceService _deviceService;
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
        public MainWindow(IDeviceService deviceService)
        {
            _deviceService = deviceService;
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
                   return await _deviceService.InitializeDeviceConnection(new AddDeviceRequest
                        {
                            DeviceId = _device.DeviceId,
                            DeviceType = _device.DeviceType,
                            Location = _device.Location,
                            Owner = _device.Owner,
                        }, _deviceClient);
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
