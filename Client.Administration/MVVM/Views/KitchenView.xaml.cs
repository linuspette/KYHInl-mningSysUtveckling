using Shared.Models.Iot;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Client.Administration.MVVM.Views
{
    /// <summary>
    /// Interaction logic for KitchenView.xaml
    /// </summary>
    public partial class KitchenView : UserControl
    {
        public KitchenView()
        {
            InitializeComponent();
            Device.IntelliTemp.MainWindow.Initialize(new IotInitialize
            {
                DeviceId = $"intelliTemp-{Guid.NewGuid()}",
                Owner = "Linus",
                Type = "Temperature Sensor"
            }).ConfigureAwait(false);
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
