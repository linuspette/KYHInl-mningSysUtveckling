namespace Client.Administration.MVVM.ViewModels;

public class LivingRoomViewModel
{
    public string Title { get; set; } = "Living Room";
    public string Temperature { get; set; } = "24";
    public string TemperatureScale { get; set; } = "°C";
    public string Humidity { get; set; } = "38";
    public string HumidityScale { get; set; } = "%";
}