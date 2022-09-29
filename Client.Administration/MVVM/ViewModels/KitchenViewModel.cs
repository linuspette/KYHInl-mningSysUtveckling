namespace Client.Administration.MVVM.ViewModels;

internal class KitchenViewModel
{
    public string Title { get; set; } = "Kitchen";
    public string Temperature { get; set; } = "23";
    public string TemperatureScale { get; set; } = "°C";
    public string Humidity { get; set; } = "33";
    public string HumidityScale { get; set; } = "%";
}