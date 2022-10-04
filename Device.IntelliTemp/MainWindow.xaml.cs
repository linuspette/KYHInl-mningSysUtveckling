using Device.IntelliTemp.Helpers;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Models.Iot;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WpfShared.Helpers;

namespace Device.IntelliTemp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IConfiguration _configuration;
        private bool timerIsOn = false;
        private readonly SolidColorBrush white = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush red = new SolidColorBrush(Colors.Red);
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public double UserSetWarningTemp { get; set; } = 30;

        public MainWindow(IConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            SendDataToIotHub().ConfigureAwait(false);

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += WarningOn;
            DataGeneration().ConfigureAwait(false);
        }

        public async Task DataGeneration()
        {
            while (true)
            {
                if (DeviceManager.isConnected)
                {
                    TemperatureGenerator.GetTemperature();
                    txtBlockTemperatureDisplay.Text = $"{Math.Round(TemperatureGenerator.TemperatureC, 0)}°C";

                    HumidityGenerator.GetHumidity();
                    txtBlockHumidityDisplay.Text = $"{Math.Round(HumidityGenerator.Humidity, 0)}%";

                    await SendDataToIotHub();

                    if (TemperatureGenerator.TemperatureC > UserSetWarningTemp && !timerIsOn)
                    {
                        timerIsOn = true;
                        timer.Start();
                    }
                    else if (TemperatureGenerator.TemperatureC < (UserSetWarningTemp-2))
                    {
                        timer.Stop();
                        timerIsOn = false;
                    }
                    await Task.Delay(10000);
                }
            }
        }
        private async Task UpdateConnectionState()
        {
            while (true)
            {
                tblockConnectionState.Text = DeviceManager.ConnectionStateMessage;

                await Task.Delay(5000);
            }
        }
        private async Task SendDataToIotHub()
        {
            try
            {
                var payload = new IntelliTempPayload
                {
                    DeviceId = DeviceManager.DeviceId,
                    Type = DeviceManager.DeviceType,
                    Temperature = TemperatureGenerator.TemperatureC,
                    Humidity = HumidityGenerator.Humidity,
                };

                var msg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)));

                await DeviceManager.SendMessageToIotHubAsync(msg);
            }
            catch { }
        }

        public static async Task Initialize(IotInitialize? iotData)
        {
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\LpSmartDevices";
            JsonSerializer serializer = new JsonSerializer();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path))
            {
                if (iotData != null)
                {
                    using (StreamWriter file = File.CreateText($"{path}\\{iotData.DeviceId}.json"))
                    {
                        serializer.Serialize(file, iotData);
                    }
                }
            }

            if (File.Exists(path))
            {
                using (StreamReader file = new StreamReader(path))
                {
                    var _iotData = JsonConvert.DeserializeObject<IotInitialize>(await file.ReadToEndAsync()) ?? null!;
                    DeviceManager.Initialize(_iotData.DeviceId, _iotData.Type, _iotData.Owner, _configuration["SysDevAzureFunctionsKey"]);
                }
            }

        }

        private void WarningOn(object? sender, EventArgs e)
        {
            txtBlockTemperatureDisplay.Foreground = txtBlockTemperatureDisplay.Foreground != red ? red : white;
        }
    }
}

