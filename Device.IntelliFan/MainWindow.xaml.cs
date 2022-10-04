using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Models.Iot;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using WpfShared.Helpers;

namespace Device.IntelliFan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConfiguration _configuration;

        public MainWindow(IConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            UpdateConnectionState().ConfigureAwait(false);
            SendDataToIotHub().ConfigureAwait(false);

            DeviceManager.Initialize("intellifan-l1001", "Fan", "Linus", _configuration["SysDevAzureFunctionsKey"]);
            DeviceManager.ConnectAsync().ConfigureAwait(false);
        }

        private static bool isRunning = false;

        private async Task UpdateConnectionState()
        {
            while (true)
            {
                tblockConnectionState.Text = DeviceManager.ConnectionStateMessage;

                if (DeviceManager.ConnectionStateMessage == "Connected")
                {
                    btnToggle.Visibility = Visibility.Visible;
                    UpdateButtonContent();
                }

                if (DeviceManager.ConnectionStateMessage == "Not connected")
                {
                    btnToggle.Visibility = Visibility.Visible;
                    btnToggle.Content = "Connect";
                }

                if (!DeviceManager.isConnected)
                {
                    btnToggle.Visibility = Visibility.Visible;
                    btnToggle.Content = "Initializing. . .";
                }

                await Task.Delay(5000);
            }
        }

        private async void btnToggle_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceManager.ConnectionStateMessage == "Not Connected")
                await DeviceManager.ConnectAsync();

            if (DeviceManager.ConnectionStateMessage == "Connected")
            {
                var sB = (BeginStoryboard)TryFindResource("sbRotate");
                if (!isRunning)
                {
                    isRunning = true;
                    sB.Storyboard.Begin();
                    UpdateButtonContent();
                }
                else
                {
                    isRunning = false;
                    sB.Storyboard.Stop();
                    UpdateButtonContent();
                }
            }
        }

        private void UpdateButtonContent()
        {
            if (!isRunning)
            {
                btnToggle.Content = "Start Fan";
            }
            else
            {
                btnToggle.Content = "Stop Fan";
            }
        }

        private async Task SendDataToIotHub()
        {
            while (true)
            {
                if (DeviceManager.isConnected)
                {
                    try
                    {
                        var payload = new IntelliFanPayload
                        {
                            DeviceId = DeviceManager.DeviceId,
                            Type = DeviceManager.DeviceType,
                            IsRunning = isRunning,
                        };

                        var msg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)));

                        await DeviceManager.SendMessageToIotHubAsync(msg);
                    }
                    catch { }
                }
                await Task.Delay(30000);
            }
        }
    }
}
