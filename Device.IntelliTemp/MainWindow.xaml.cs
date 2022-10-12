using Device.IntelliTemp.Helpers;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Shared.Models.Iot;
using System;
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
        private bool timerIsOn = false;
        private readonly SolidColorBrush white = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush red = new SolidColorBrush(Colors.Red);
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public double UserSetWarningTemp { get; set; } = 30;

        public MainWindow()
        {
            InitializeComponent();

            DeviceManager.Initialize(new DeviceSettings
            {
                DeviceId = "intelliTemp-f1002",
                DeviceName = "IntelliTemp",
                Owner = "Linus",
                Location = "Living Room",
                DeviceType = "TempSensor",
                Interval = 10000,
                DeviceState = false
            });
            Task.Run(DeviceManager.ConnectAsync);

            UpdateConnectionStateAsync().ConfigureAwait(false);
            DeviceManager.SetDirectMethodAsync().ConfigureAwait(false);

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += WarningOn;
            DataGenerationAsync().ConfigureAwait(false);

        }

        public async Task DataGenerationAsync()
        {
            while (true)
            {
                if (DeviceManager.isConnected)
                {
                    TemperatureGenerator.GetTemperature();
                    txtBlockTemperatureDisplay.Text = $"{Math.Round(TemperatureGenerator.TemperatureC, 0)}°C";

                    HumidityGenerator.GetHumidity();
                    txtBlockHumidityDisplay.Text = $"{Math.Round(HumidityGenerator.Humidity, 0)}%";

                    await SendDataAsync();

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
                else
                {
                    await Task.Delay(1000);
                }
            }
        }
        private async Task UpdateConnectionStateAsync()
        {
            while (true)
            {
                tblockConnectionState.Text = DeviceManager.ConnectionStateMessage;

                await Task.Delay(5000);
            }
        }
        private async Task SendDataAsync()
        {
            try
            {
                if (DeviceManager.isConnected)
                {
                    var payload = new IntelliTempPayload
                    {
                        DeviceId = DeviceManager._deviceSettings.DeviceId,
                        Type = DeviceManager._deviceSettings.DeviceType,
                        Temperature = TemperatureGenerator.TemperatureC,
                        Humidity = HumidityGenerator.Humidity,
                    };

                    var msg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)));

                    await DeviceManager.SendMessageToIotHubAsync(msg);
                }
            }
            catch { }
        }
        private void WarningOn(object? sender, EventArgs e)
        {
            txtBlockTemperatureDisplay.Foreground = txtBlockTemperatureDisplay.Foreground != red ? red : white;
        }
    }
}

