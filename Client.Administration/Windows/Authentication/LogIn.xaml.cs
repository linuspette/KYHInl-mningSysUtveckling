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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseBtnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void ResizeBtnClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                btnResize.Content = FindResource("Restore");
            }
            else
            {
                this.WindowState = WindowState.Normal;
                btnResize.Content = FindResource("Maximize");
            }
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
                MessageBox.Show("Log in succeded", "Success", MessageBoxButton.OK);
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
