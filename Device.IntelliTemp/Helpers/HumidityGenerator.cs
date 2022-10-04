using System;

namespace Device.IntelliTemp.Helpers;

public class HumidityGenerator
{
    private static Random random = new Random();
    private static double minChange = -0.25;
    private static double maxChange = 0.25;

    private static double minHum = 1.0;
    private static double maxHum = 100;

    private static bool validHum = true;

    public static double Humidity { get; set; } = 32;

    public static void GetHumidity()
    {
        double hum = 0;

        do
        {
            hum = GenerateHumidity();
            validHum = ValidateHumidity(hum);
        } while (!validHum);

        Humidity = hum;
    }

    private static bool ValidateHumidity(double newHum)
    {
        if (newHum >= minHum && newHum <= maxHum)
            return true;
        return false;
    }

    private static double GenerateHumidity()
    {
        return (Humidity + (random.NextDouble() * ((maxChange * 1.7895) - minChange) + minChange));
    }
}