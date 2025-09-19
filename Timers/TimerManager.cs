namespace FluffyVoid.Timers;

/// <summary>
///     Manager class for timers that is responsible for updating timers
/// </summary>
public static class TimerManager
{
    /// <summary>
    ///     Lock object for basic locking operations
    /// </summary>
    private static readonly object LockObject = new object();
    /// <summary>
    ///     Lock object for queue of timers to destroy
    /// </summary>
    private static readonly object QueueLock = new object();
    /// <summary>
    ///     List of timers currently registered
    /// </summary>
    private static readonly List<Timer> Timers = new List<Timer>();
    /// <summary>
    ///     List of timers queued to be destroyed
    /// </summary>
    private static readonly Queue<Timer> TimersToDestroy = new Queue<Timer>();

    /// <summary>
    ///     Adds a timer to be managed by the manager
    /// </summary>
    /// <param name="timer">The timer to add</param>
    public static void AddTimer(Timer timer)
    {
        lock (LockObject)
        {
            if (!Timers.Contains(timer))
            {
                Timers.Add(timer);
            }
        }
    }
    /// <summary>
    ///     Removes a timer from the manager
    /// </summary>
    /// <param name="timer">The timer to remove</param>
    public static void RemoveTimer(Timer timer)
    {
        lock (QueueLock)
        {
            timer.IsEnabled = false;
            TimersToDestroy.Enqueue(timer);
        }
    }

    /// <summary>
    ///     Updates all active timers within the manager
    /// </summary>
    /// <param name="deltaTime">The time since the last update call</param>
    public static void Update(float deltaTime)
    {
        IncrementTimers(deltaTime);
        lock (QueueLock)
        {
            while (TimersToDestroy.Any())
            {
                Timer toRemove = TimersToDestroy.Dequeue();
                Timers.Remove(toRemove);
            }
        }
    }

    /// <summary>
    ///     Helper function that iterates through the active timers and increments their elapsed times, Ticking any that
    ///     qualify
    /// </summary>
    /// <param name="deltaTime">The time since the last update call</param>
    private static void IncrementTimers(float deltaTime)
    {
        Timer[] enabledTimers;
        lock (LockObject)
        {
            enabledTimers = Timers.Where(t => t.IsEnabled).ToArray();
        }

        foreach (Timer timer in enabledTimers)
        {
            timer.IncrementTime(deltaTime);
            if (timer.Elapsed < timer.Interval)
            {
                continue;
            }

            float totalElapsed = timer.Elapsed;
            timer.DecrementTime(timer.Interval);
            timer.OnTimerTicked(new TimerTickedArgs(timer, totalElapsed));
        }
    }
}