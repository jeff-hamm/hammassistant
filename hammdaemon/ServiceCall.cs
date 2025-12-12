using NetDaemon.Client.HomeAssistant.Model;

namespace NetDaemon.Extensions.Hammlet.Services;

public record ServiceCall(string Domain, string Service, HassTarget? Target = null, object? Data = null)
{
    public DateTimeOffset SentTime { get; set; }
}