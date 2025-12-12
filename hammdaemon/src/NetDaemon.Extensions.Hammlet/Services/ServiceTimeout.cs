namespace NetDaemon.Extensions.Hammlet.Services;

public class ServiceTimeout(TimeSpan initialTimeout, TimeSpan maxTimeout, double increaseFactor = 2.0)
{
    private TimeSpan? _currentTimeout;
    public TimeSpan NextTimeout()
    {
        var timeSpan = _currentTimeout == null ? initialTimeout : _currentTimeout * increaseFactor;
        _currentTimeout = timeSpan > maxTimeout ? maxTimeout : timeSpan;
        return _currentTimeout.Value;
    }

    public TimeSpan? PreviousTimeout => _currentTimeout / increaseFactor;

    public int MaxRetryAttempts => (int)Math.Log(maxTimeout.TotalMilliseconds / initialTimeout.TotalMilliseconds, increaseFactor);
    public ServiceTimeout(TimeoutConfig cfg) : this(cfg.InitialTimeout, cfg.MaxTimeout ?? TimeSpan.MaxValue, cfg.IncreaseFactor) { }
    public ServiceTimeout(TimeSpan initialTimeout, int? maxTimoutMs = null, double increaseFactor = 2.0) : this(initialTimeout, maxTimoutMs.HasValue ? TimeSpan.FromMilliseconds(maxTimoutMs.Value) : TimeSpan.MaxValue, increaseFactor) { }
    public ServiceTimeout(int initialTimeoutMs, int? maxTimoutMs = null, double increaseFactor = 2.0) : this(TimeSpan.FromMilliseconds(initialTimeoutMs), maxTimoutMs, increaseFactor) { }
    public static ServiceTimeout Default => new(new TimeoutConfig(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60), 2.0));

    public static ServiceTimeout ForRetries(TimeSpan initialTimeout, int maxRetries, double increaseFactor = 2.0) =>
        new ServiceTimeout(initialTimeout, TimeSpan.FromMilliseconds(initialTimeout.Milliseconds * Math.Pow(increaseFactor, maxRetries)), increaseFactor);
}