using System;

namespace Device.IntelliTemp.Helpers;

public static class TemperatureGenerator
{

    private static Random random = new Random();
    private static double minChange = -0.5;
    private static double maxChange = 0.5;


    public static double TemperatureC { get; set; } = 20;
    public static double TemperatureF { get; set; } = ConvertToFahrenheit();

    public static void GenerateTemperature()
    {
        var temp = (TemperatureC + (random.NextDouble() * (maxChange - (minChange)) + minChange));

        TemperatureC = temp;
        TemperatureF = ConvertToFahrenheit();
    }

    private static double ConvertToFahrenheit()
    {
        return ((TemperatureC * (double)9/5) + 32);
    }
}