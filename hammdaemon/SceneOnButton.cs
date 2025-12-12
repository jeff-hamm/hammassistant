using System.Linq;
using System.Text.Json;
using Hammlet.NetDaemon.Models;
using HassModel;
using Hammlet.Models.Extensions;
using NetDaemon.Extensions.Hammlet;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Apps.SceneOnButton;

[NetDaemonApp]
public class SceneOnButton
{
    private static readonly JsonSerializerOptions o = new()
    {
        WriteIndented = true,
    };

    public SceneOnButton(IHaContext ha, IEntityFactory entityFactory, EventEntities events, LightEntities lights, ILogger<SceneOnButton> logger,
        IAppConfig<SceneSelectorConfig> config)
    {
        var buttonEvents = ha.Events(config.Value.ButtonEventIds)
            .StateChanges()
            .Subscribe(e =>
            {

                var light = lights.Entity(config.Value.TargetEntityId);
                foreach (var button in config.Value.Buttons.Where(b =>
                             e.Entity.EventType<ButtonEventType>() == b.Arg &&
                             e.Entity.EntityId.EndsWith(b.EventIndex)))
                {

                    logger.LogInformation("Remote control event: {event}, performing action {action}", e.Entity.EntityId, button.Action);
                    switch (button.Action)
                    {
                        case ButtonAction.Brighten:
                        case ButtonAction.Darken:
                            if (button.On is not { } param)
                            {
                                param = new LightTurnOnParameters()
                                {
                                    BrightnessStepPct = config.Value.BrightnessPct *
                                                        (button.Action == ButtonAction.Brighten ? 1.0 : -1.0)
                                };
                            }
                            else
                            {
                                param = param with
                                {
                                    BrightnessStepPct = config.Value.BrightnessPct *
                                                        (button.Action == ButtonAction.Brighten ? 1.0 : -1.0)
                                };
                            }
                            if (button.Action == ButtonAction.Brighten && (light.Attributes?.Brightness == null || light.Attributes.Brightness == 0))
                                param = param with
                                {
                                    BrightnessPct = 25,
                                    BrightnessStepPct = null
                                };

                            light.TurnOn(param);
                            break;
                        case ButtonAction.ToggleWarm:
                            light.ToggleWarm();
                            break;
                        case ButtonAction.TurnOff:
                            light.TurnOff();
                            break;
                        case ButtonAction.TurnOn:
                            light.TurnOn();
                            break;
                        case ButtonAction.Toggle:
                            light.Toggle();
                            break;
                    }
                }
            });
    }
}
