using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Device.IntelliTemp.Helpers;

namespace Device.IntelliTemp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool timerIsOn = false;
        private SolidColorBrush white = new SolidColorBrush(Colors.White);
        private SolidColorBrush red = new SolidColorBrush(Colors.Red);
        private DispatcherTimer timer = new DispatcherTimer();

        public double UserSetWarningTemp { get; set; } = 30;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += WarningOn;
            TemperatureGeneration().ConfigureAwait(false);
        }

        public async Task TemperatureGeneration()
        {
            while (true)
            {
                TemperatureGenerator.GenerateTemperature();
                txtBlockTemperatureDisplay.Text = $"{Math.Round(TemperatureGenerator.TemperatureC, 0)}°C";
                if (TemperatureGenerator.TemperatureC > UserSetWarningTemp && !timerIsOn)
                {
                    timerIsOn = true;
                    timer.Start();
                }
                else if(TemperatureGenerator.TemperatureC < (UserSetWarningTemp-2))
                {
                    timer.Stop();
                    timerIsOn = false;
                }
                await Task.Delay(500);
            }
        }

        private void WarningOn(object? sender, EventArgs e)
        {
            txtBlockTemperatureDisplay.Foreground = txtBlockTemperatureDisplay.Foreground != red ? red : white;
        }

        private void BtnClick_FanSwitch(object sender, RoutedEventArgs e)
        {
            if (!TemperatureGenerator.fanIsOn)
            {
                btnFanControl.Content = "Shut off fan";
                TemperatureGenerator.fanIsOn = true;
            }            
            else if (TemperatureGenerator.fanIsOn)
            {
                btnFanControl.Content = "Start fan";
                TemperatureGenerator.fanIsOn = false;
            }
        }
    }
}
