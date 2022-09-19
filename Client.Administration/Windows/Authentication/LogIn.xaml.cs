using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.Administration.Helpers;
using Client.Administration.Windows.Devices;
using Shared.Models.Input.Users;

namespace Client.Administration.Windows.Authentication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private readonly IApiClient _apiClient;
        private readonly ITokenManager _tokenManager;

        public LogIn(IApiClient apiClient, ITokenManager tokenManager)
        {
            _apiClient = apiClient;
            _tokenManager = tokenManager;
            InitializeComponent();
        }

        private void MinimizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private async void SignInAsync(object sender, RoutedEventArgs e)
        {
            var result = await _apiClient.SignInAsync(new SignIn
            {
                Username = txtBoxUsername.Text,
                Password = txtBoxPassword.Password
            });

            if (result)
            {
                var mainWindow = new MainWindow(_apiClient, this);
                Hide();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Log in failed", "Failed", MessageBoxButton.OK);
            }
        }

        private void BtnRegister(object sender, RoutedEventArgs e)
        {
            var register = new Register(_apiClient, this);
            register.Show();
            Hide();
        }
    }
}
