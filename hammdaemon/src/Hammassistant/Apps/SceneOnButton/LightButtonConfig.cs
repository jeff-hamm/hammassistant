using Hammlet.NetDaemon.Models;

namespace Hammlet.Apps.SceneOnButton;

public partial record LightButtonConfig
{
    public ButtonAction Action { get; set; } = ButtonAction.Toggle;
    public LightTurnOnParameters? On { get; set; }
    public LightTurnOffParameters? Off { get; set; }
    public string EventIndex { get; set; } = "000";
    public ButtonEventType Arg { get; set; } = ButtonEventType.Pressed;
}