using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Device.IntelliTemp.Helpers;

namespace Device.IntelliTemp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TemperatureGeneration().ConfigureAwait(false);
        }

        public async Task TemperatureGeneration()
        {
            while (true)
            {
                TemperatureGenerator.GenerateTemperature();
                txtBlockTemperatureDisplay.Text = $"{Math.Round(TemperatureGenerator.TemperatureC, 1)}°C";
                await Task.Delay(5000);
            }
        }
    }
}
