using System;
using System.Windows.Threading;

namespace LpSmartHub.Services;

public abstract class Timers
{
    public Timers()
    {
        InitializeTimers();
    }

    protected virtual void InitializeTimers()
    {
        DispatcherTimer ClockTimer = new DispatcherTimer();
        ClockTimer.Interval = TimeSpan.FromSeconds(7);
        ClockTimer.Tick += ClockTimer_Tick;
        ClockTimer.Start();
    }

    protected virtual void ClockTimer_Tick(object? sender, EventArgs e)
    {

    }
}