using System.Collections.Generic;

namespace Client.Administration.MVVM.ViewModels;

internal class KitchenViewModel
{
    public string Title { get; set; } = "Kitchen";
    public List<Microsoft.Azure.Devices.Device> Devices { get; set; } = new List<Microsoft.Azure.Devices.Device>();
}