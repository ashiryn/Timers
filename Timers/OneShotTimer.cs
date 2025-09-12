namespace FluffyVoid.Timers;

/// <summary>
///     Event timer that will run exactly once, and then remove itself from the timer manager
///     Useful for time based one-off event firing
/// </summary>
public class OneShotTimer : Timer
{
    /// <summary>
    ///     Default constructor that sets a timer to 1 second interval and in an OFF state
    /// </summary>
    public OneShotTimer()
    {
    }
    /// <summary>
    ///     Constructor that sets the interval to the passed in value
    /// </summary>
    /// <param name="interval">The number of seconds to wait between Ticks</param>
    public OneShotTimer(float interval)
        : base(interval)
    {
    }
    /// <summary>
    ///     Constructor that sets the interval to the passed in value, and the id of the timer
    /// </summary>
    /// <param name="interval">The number of seconds to wait between Ticks</param>
    /// <param name="id">The id of the timer to use for future lookups</param>
    public OneShotTimer(float interval, int id)
        : base(interval, id)
    {
    }

    /// <summary>
    ///     Tick handler that forwards to any registered event handlers
    /// </summary>
    /// <param name="e">Event args containing information from the timer</param>
    internal override void OnTimerTicked(TimerTickedArgs e)
    {
        base.OnTimerTicked(e);
        TimerManager.RemoveTimer(this);
    }
}