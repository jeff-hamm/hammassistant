namespace NetDaemon.Extensions.Hammlet.Services;

public record class TimeoutConfig(TimeSpan InitialTimeout, TimeSpan? MaxTimeout = null, double IncreaseFactor = 2.0);