using Hammlet.Extensions;
using Hammlet.NetDaemon.Models;
using NetDaemon.Extensions.Hammlet;

namespace Hammlet;

public class DimmerSync
{
    public string DimmerId { get; set; } = "light.liminal_dimmer";
    public int PressBrightness { get; set; } = 10;
    public int TickBrightness { get; set; } = 15;
    public string TargetLightId { get; set; } = "light.liminal_ceiling";
    public string? UpSensorId { get; set; }
    public string? DownSensorId { get; set; }
    public int DimmerDelay { get; set; } = 80;

    public ButtonTiming Timing { get; set; } = new ButtonTiming();
    public string? UpEventId { get; set; }
    public string? DownButtonId { get; set; }
}

