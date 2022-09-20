using System;

namespace Device.IntelliTemp.Helpers;

public static class TemperatureGenerator
{

    private static Random random = new Random();
    private static double minChange = -0.25;
    private static double maxChange = 0.25;

    private static double minTemp = 16;
    private static double maxTemp = 38;

    private static double fanOnMultiplier = 1.35;
    private static double fanOffMultiplier = 1.5;

    private static bool validTemp = true;
    public static bool fanIsOn = false;

    public static double TemperatureC { get; set; } = 20;
    public static double TemperatureF { get; set; } = ConvertToFahrenheit();

    public static void GenerateTemperature()
    {
        double temp = 0;

        do
        {
            temp = GetTemperature();
            validTemp = ValidateTemperature(temp);
        } while (!validTemp);

        TemperatureC = temp;
        TemperatureF = ConvertToFahrenheit();
    }

    private static bool ValidateTemperature(double newTemp)
    {
        if (newTemp >= minTemp && newTemp <= maxTemp)
            return true;

        return false;
    }

    private static double ConvertToFahrenheit()
    {
        return ((TemperatureC * (double)9/5) + 32);
    }

    private static double GetTemperature()
    {
        if (fanIsOn)
        {
            var temp = (random.NextDouble() * ((maxChange * fanOnMultiplier) - (minChange / fanOnMultiplier)) + minChange);
            return (TemperatureC - (temp * fanOnMultiplier));
        }

        return (TemperatureC + (random.NextDouble() * ((maxChange * fanOffMultiplier) - minChange) + minChange));
    }
}