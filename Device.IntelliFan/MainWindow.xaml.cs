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
        public MainWindow()
        {
            InitializeComponent();
            UpdateConnectionState().ConfigureAwait(false);

            DeviceManager.Initialize("intellifan-l1001", "Fan", "Linus");
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
    }
}
