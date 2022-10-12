using System;
using System.Windows.Threading;

namespace Client.Administration.Services;

internal abstract class Timers
{
    public Timers()
    {
        InitializeTimers();
    }

    protected virtual void InitializeTimers()
    {
        DispatcherTimer ClockTimer = new DispatcherTimer();
        ClockTimer.Interval = TimeSpan.FromMinutes(1);
        ClockTimer.Tick += ClockTimer_Tick;
        ClockTimer.Start();

        DispatcherTimer TemperatureTimer = new DispatcherTimer();
        TemperatureTimer.Interval = TimeSpan.FromMinutes(30);
        TemperatureTimer.Tick += TemperatureTimer_Tick;
        TemperatureTimer.Start();
    }

    protected virtual void TemperatureTimer_Tick(object? sender, EventArgs e)
    {
    }

    protected virtual void ClockTimer_Tick(object? sender, EventArgs e)
    {
    }
}