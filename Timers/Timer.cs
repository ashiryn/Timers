namespace FluffyVoid.Timers;

/// <summary>
///     Event timer that allows for a callback to be called whenever the timer Ticks its interval
/// </summary>

// ReSharper disable once ClassNeverInstantiated.Global
public class Timer : IDisposable
{
    /// <summary>
    ///     The amount of time that has passed since the last Tick
    /// </summary>
    public float Elapsed { get; set; }

    /// <summary>
    ///     The id of the timer
    /// </summary>

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int Id { get; set; }
    /// <summary>
    ///     The time between Ticks
    /// </summary>
    public float Interval { get; set; }
    /// <summary>
    ///     Whether the timer is currently enabled or not
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    ///     Callbacks when the timer has ticked past its set interval
    /// </summary>
    public event EventHandler<TimerTickedArgs>? Ticked;

    /// <summary>
    ///     Default constructor that sets a timer to 1 second interval and in an OFF state
    /// </summary>
    public Timer()
    {
        Id = -1;
        Interval = 1;
        Elapsed = 0;
        IsEnabled = false;
        TimerManager.AddTimer(this);
    }
    /// <summary>
    ///     Constructor that sets the interval to the passed in value
    /// </summary>
    /// <param name="interval">The number of seconds to wait between Ticks</param>
    public Timer(float interval)
    {
        Id = -1;
        Interval = interval;
        Elapsed = 0;
        IsEnabled = false;
        TimerManager.AddTimer(this);
    }
    /// <summary>
    ///     Constructor that sets the interval to the passed in value, and the id of the timer
    /// </summary>
    /// <param name="interval">The number of seconds to wait between Ticks</param>
    /// <param name="id">The id of the timer to use for future lookups</param>
    public Timer(float interval, int id)
    {
        Id = id;
        Interval = interval;
        Elapsed = 0;
        IsEnabled = false;
        TimerManager.AddTimer(this);
    }

    /// <summary>
    ///     Cleans up any resources allocated by the Timer
    /// </summary>
    public void Dispose()
    {
        TimerManager.RemoveTimer(this);
    }
    /// <summary>
    ///     Resets the currently elapsed time.
    /// </summary>
    public void Reset()
    {
        Elapsed = 0.0f;
    }

    /// <summary>
    ///     Starts the timer
    /// </summary>
    public void Start()
    {
        IsEnabled = true;
    }
    /// <summary>
    ///     Stops the timer, retaining the currently elapsed time. Call Reset before the next Start in order to get a fresh
    ///     Tick, otherwise will resume
    /// </summary>
    public void Stop()
    {
        IsEnabled = false;
    }
    /// <summary>
    ///     Decrements the elapsed time by the amount passed in
    /// </summary>
    /// <param name="time">The amount of time to decrement from the currently elapsed time</param>
    internal void DecrementTime(float time)
    {
        Elapsed -= time;
    }
    /// <summary>
    ///     Increments the elapsed time by the amount passed in
    /// </summary>
    /// <param name="time">The amount of time to add to the currently elapsed time</param>
    internal void IncrementTime(float time)
    {
        Elapsed += time;
    }

    /// <summary>
    ///     Tick handler that forwards to any registered event handlers
    /// </summary>
    /// <param name="e">Event args containing information from the timer</param>
    internal virtual void OnTimerTicked(TimerTickedArgs e)
    {
        Ticked?.Invoke(this, e);
    }
}