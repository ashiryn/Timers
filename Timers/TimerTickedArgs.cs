namespace FluffyVoid.Timers;

/// <summary>
///     Event arg class for when a Timer has Ticked
/// </summary>
public class TimerTickedArgs : EventArgs
{
    /// <summary>
    ///     Reference to the timer that is currently Ticking
    /// </summary>

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Timer Timer { get; }
    /// <summary>
    ///     The amount of time since that last Tick of the timer
    /// </summary>

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public float TimeSinceLastTick { get; }

    /// <summary>
    ///     Default constructor for the event arg
    /// </summary>
    /// <param name="timer">Reference to the timer that is currently Ticking</param>
    /// <param name="timeSinceLastTick">The amount of time since that last Tick of the timer</param>
    public TimerTickedArgs(Timer timer, float timeSinceLastTick)
    {
        Timer = timer;
        TimeSinceLastTick = timeSinceLastTick;
    }
}