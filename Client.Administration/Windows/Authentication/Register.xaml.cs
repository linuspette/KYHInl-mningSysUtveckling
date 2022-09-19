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
using System.Windows.Shapes;
using Client.Administration.Helpers;
using Shared.Models.Input.Users;

namespace Client.Administration.Windows.Authentication
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private readonly IApiClient _apiClient;
        private readonly LogIn _logInWindow;

        public Register(IApiClient apiClient, LogIn logInWindow)
        {
            _apiClient = apiClient;
            _logInWindow = logInWindow;
            InitializeComponent();
        }
        private void MinimizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private async void BtnRegisterAsync(object sender, RoutedEventArgs e)
        {
            if (txtBoxPassword.Password != txtBoxPasswordMatching.Password)
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK);
            var result = await _apiClient.SignUpAsync(new SignUp
            {
                Username = txtBoxUsername.Text,
                Password = txtBoxPassword.Password
            });

            if (result.Suceeded)
            {
                _logInWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show($"{result.Message}", "Failed", MessageBoxButton.OK);
            }
        }
    }
}
