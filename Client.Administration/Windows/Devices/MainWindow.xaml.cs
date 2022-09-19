using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.Administration.Helpers;
using Client.Administration.Windows.Authentication;
using Shared.Models.View.Device;

namespace Client.Administration.Windows.Devices
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IApiClient _apiClient;
        private readonly LogIn _logInWindow;

        public MainWindow(IApiClient apiClient, LogIn logInWindow)
        {
            _apiClient = apiClient;
            _logInWindow = logInWindow;
            InitializeComponent();

            CheckTokenValidityAsync().ConfigureAwait(false);
            GetDevicesAsync().ConfigureAwait(false);

        }

        public List<IotDevice> IotDevices { get; private set; } = null!;

        //Checks token-validity every 60 seconds
        public async Task CheckTokenValidityAsync()
        {
            bool tokenValidity = true;

            while (tokenValidity)
            {
                tokenValidity = await _apiClient.ValidateTokenAsync();

                await Task.Delay(60000);
            }

            Hide();
            _logInWindow.Show();
            Close();
        }

        public async Task GetDevicesAsync()
        {

        }
    }
}
