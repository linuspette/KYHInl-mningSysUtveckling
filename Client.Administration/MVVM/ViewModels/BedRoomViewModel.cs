namespace Client.Administration.MVVM.ViewModels;

public class BedRoomViewModel
{
    public string Title { get; set; } = "Bed Room";
    public string Temperature { get; set; } = "19";
    public string TemperatureScale { get; set; } = "°C";
    public string Humidity { get; set; } = "28";
    public string HumidityScale { get; set; } = "%";
}