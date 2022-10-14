using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace LpSmartHub.Components
{
    /// <summary>
    /// Interaction logic for ConfigureIotDeviceDialog.xaml
    /// </summary>
    public partial class ConfigureIotDeviceDialog : Window
    {
        private readonly Regex _regex = new Regex("[^0-9.-]+");

        public ConfigureIotDeviceDialog(int intervalDefault)
        {
            InitializeComponent();
            txtInterval.Text = intervalDefault.ToString();
        }

        private void ValidateInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }


        private void BtnDialogOk_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtInterval.SelectAll();
            txtInterval.Focus();
        }

        public string Interval
        {
            get { return txtInterval.Text; }
        }
    }
}
