using System.Diagnostics;

namespace FluffyVoid.Game;

/// <summary>
///     Global time keeper for calculating Delta Time
/// </summary>
public class Time
{
    /// <summary>
    ///     Singleton instance for the time class
    /// </summary>
    private static Time? s_instance;
    /// <summary>
    ///     Stopwatch to use for calculating the delta time
    /// </summary>
    private readonly Stopwatch _clock = new Stopwatch();
    /// <summary>
    ///     The current frames delta time
    /// </summary>
    private double _deltaTime;
    /// <summary>
    ///     The desired framerate the application is attempting to maintain
    /// </summary>
    private double _desiredFramerate = 1.0 / 60.0;
    /// <summary>
    ///     The amount of time that has elapsed to calculate FPS
    /// </summary>
    private float _elapsed;
    /// <summary>
    ///     The number of frames that have currently been run
    /// </summary>
    private int _frames;
    /// <summary>
    ///     Whether Time is running or not
    /// </summary>
    private bool _isRunning;
    /// <summary>
    ///     The previous frames elapsed ticks
    /// </summary>
    private long _lastElapsedMilliseconds;
    /// <summary>
    ///     The amount of time the last frame was delayed to meet the desired framerate
    /// </summary>
    private int _previousFrameDelay;

    /// <summary>
    ///     The current delta time
    /// </summary>
    public static float DeltaTime => (float)Instance._deltaTime;
    /// <summary>
    ///     The current Frames per Second the application is running at
    /// </summary>
    public static int FPS { get; private set; }
    /// <summary>
    ///     Whether Time is running or not
    /// </summary>
    public bool IsRunning => Instance._isRunning;
    /// <summary>
    ///     Singleton instance for the time class
    /// </summary>
    private static Time Instance => s_instance ??= new Time();

    /// <summary>
    ///     Constructor for setting up the Time class
    /// </summary>
    private Time()
    {
    }

    /// <summary>
    ///     Calculates the amount of milliseconds to wait in order to maintain a locked FPS
    /// </summary>
    /// <param name="targetFramerate">The target framerate to lock to</param>
    /// <returns>The amount of milliseconds to wait</returns>
    public static int Delay(double targetFramerate)
    {
        if (!(targetFramerate > 0.0) || !Instance._isRunning)
        {
            return 0;
        }

        Instance._desiredFramerate = 1.0 / targetFramerate;
        double previousFrameDelayInSeconds =
            Instance._previousFrameDelay * 0.001;

        double frameDelayAdjustment =
            Instance._desiredFramerate - Instance._deltaTime;

        double currentFrameDelay =
            previousFrameDelayInSeconds + frameDelayAdjustment;

        int result = Math.Max(0, (int)(currentFrameDelay * 1000));
        Instance._previousFrameDelay = result;
        return result;
    }
    /// <summary>
    ///     Resets the time to get it back into its default state
    /// </summary>
    public static void Reset()
    {
        Instance._clock.Reset();
        Instance._deltaTime = 0;
        Instance._previousFrameDelay = 0;
        Instance._lastElapsedMilliseconds = 0;
    }
    /// <summary>
    ///     Signals the time to calculate delta time and FPS
    /// </summary>
    public static void Signal()
    {
        if (!Instance._isRunning)
        {
            return;
        }

        long currentElapsedMilliseconds = Instance._clock.ElapsedMilliseconds;
        long elapsedSinceLastDelta = currentElapsedMilliseconds -
                                     Instance._lastElapsedMilliseconds;

        Instance._lastElapsedMilliseconds = currentElapsedMilliseconds;
        Instance._deltaTime = elapsedSinceLastDelta * 0.001;
        Instance._elapsed += DeltaTime;
        if (Instance._elapsed >= 1.0f)
        {
            Instance._elapsed -= 1.0f;
            FPS = Instance._frames;
            Instance._frames = 0;
        }

        ++Instance._frames;
    }
    /// <summary>
    ///     Starts the time ticking
    /// </summary>
    public static void Start()
    {
        Instance._clock.Start();
        Instance._isRunning = true;
    }
    /// <summary>
    ///     Stops the time from ticking
    /// </summary>
    public static void Stop()
    {
        Instance._clock.Stop();
        Instance._isRunning = false;
    }
}