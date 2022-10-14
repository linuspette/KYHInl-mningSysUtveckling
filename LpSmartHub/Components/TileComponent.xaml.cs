using LpSmartHub.MVVM.Models;
using LpSmartHub.Services;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LpSmartHub.Components
{
    /// <summary>
    /// Interaction logic for TileComponent.xaml
    /// </summary>
    public partial class TileComponent : UserControl, INotifyPropertyChanged
    {
        private readonly IDeviceService _deviceService;

        public TileComponent()
        {
            _deviceService = new DeviceService();
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Device Name
        private static readonly DependencyProperty DeviceIdProperty = DependencyProperty.Register("DeviceId", typeof(string), typeof(TileComponent));
        public string DeviceId
        {
            get { return (string)GetValue(DeviceIdProperty); }
            set { SetValue(DeviceIdProperty, value); }
        }
        //Device Name
        private static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register("DeviceName", typeof(string), typeof(TileComponent));
        public string DeviceName
        {
            get { return (string)GetValue(DeviceNameProperty); }
            set { SetValue(DeviceNameProperty, value); }
        }

        //Device Type
        private static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register("DeviceType", typeof(string), typeof(TileComponent));
        public string DeviceType
        {
            get { return (string)GetValue(DeviceTypeProperty); }
            set { SetValue(DeviceTypeProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(TileComponent));
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }


        public static readonly DependencyProperty IconActiveProperty = DependencyProperty.Register("IconActive", typeof(string), typeof(TileComponent));

        public string IconActive
        {
            get { return (string)GetValue(IconActiveProperty); }
            set { SetValue(IconActiveProperty, value); }
        }

        public static readonly DependencyProperty IconInActiveProperty = DependencyProperty.Register("IconInActive", typeof(string), typeof(TileComponent));


        public string IconInActive
        {
            get { return (string)GetValue(IconInActiveProperty); }
            set { SetValue(IconInActiveProperty, value); }
        }

        private bool _deviceState;

        public bool DeviceState
        {
            get { return _deviceState; }
            set
            {
                _deviceState = value;
                OnPropertyChanged();
            }
        }

        private async Task ChangeDeviceStateAsync(string deviceId)
        {
            await _deviceService.SendDirectMethodAsync(new DirectMethodRequest
            {
                DeviceId = deviceId,
                MethodName = "OnOff",
                Payload = new { deviceState = IsChecked }
            });
        }

        private async void DeviceTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var deviceItem = (DeviceItem)button!.DataContext;
                IsChecked = deviceItem.DeviceState;
                DeviceState = IsChecked;

                using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(_deviceService.GetIotHubConnectionString());

                var directMethod = new CloudToDeviceMethod("OnOff");
                directMethod.SetPayloadJson(JsonConvert.SerializeObject(new { deviceState = IsChecked }));
                var result = await serviceClient.InvokeDeviceMethodAsync(deviceItem.DeviceId, directMethod);
            }
            catch { }
        }

        private async void RemoveIotDeviceButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var deviceItem = (DeviceItem)button!.DataContext;

            await _deviceService.RemoveIotDeviceAsync(deviceItem.DeviceId);
        }

        private async void ConfigureIotDeviceButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var deviceItem = (DeviceItem)button!.DataContext;


                var dialog = new ConfigureIotDeviceDialog(deviceItem.Interval);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {


                        using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(_deviceService.GetIotHubConnectionString());

                        var directMethod = new CloudToDeviceMethod("ChangeInterval");
                        directMethod.SetPayloadJson(JsonConvert.SerializeObject(new { interval = dialog.Interval }));
                        var result = await serviceClient.InvokeDeviceMethodAsync(deviceItem.DeviceId, directMethod);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
