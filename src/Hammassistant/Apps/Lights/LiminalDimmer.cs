using System.Reactive.Concurrency;
using Hammlet.Apps.SceneOnButton;
using Hammlet.Extensions;
using Hammlet.NetDaemon.Models;
using HassModel;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Apps.Lights;
#if HAMMLET
[NetDaemonApp]
internal class LiminalDimmer
{
    public LiminalDimmer(IHaContext ha, LightEntities entities,BinarySensorEntities buttons, EventEntities events, ILogger<LiminalDimmer> logger, IScheduler scheduler)
    {
        //buttons.LiminalDimmerInput0.StateChanges().Subscribe(s =>
        //{
        //    logger.LogInformation("1 Input: {0}", s.New?.State);
        //});
        //buttons.LiminalDimmerInput1.StateChanges().Subscribe(s =>
        //{
        //    logger.LogInformation("2 Input: {0}", s.New?.State);
        //});
        buttons.LiminalDimmerInput0.ToButtonEvents(scheduler).Subscribe(e =>
        {
            if (e.Data.EventType == ButtonEventType.Pressed)
            {
                if (entities.LiminalCeiling.IsOff())
                {
                    entities.LiminalCeiling.TurnOn();
                }
                else
                {
                    entities.LiminalCeiling.Brighten();
                }
            }
            if (e.Data.EventType == ButtonEventType.KeyPressed2x)
            {
                entities.LiminalCeiling.ToggleWarm();
            }
        });
        buttons.LiminalDimmerInput1.ToButtonEvents(scheduler).Subscribe(e =>
        {
            logger.LogInformation("1 Input: {0}", e.Data);
            if (e.Data.EventType == ButtonEventType.Pressed)
            {
                if (entities.LiminalCeiling.IsOn())
                {
                    entities.LiminalDimmer.TurnOff();
                }
                else
                {
                    entities.LiminalCeiling.Darken();
                }
            }
            if (e.Data.EventType == ButtonEventType.KeyPressed2x)
            {
                entities.LiminalCeiling.ToggleWarm();
            }
        });
        entities.LiminalDimmer.StateAllChanges().Subscribe(s =>
        {
            if (entities.LiminalDimmer.IsOn() && s.Entity?.EntityState?.Attributes?.Brightness is { } b)
            {
                entities.LiminalCeiling.TurnOn(new()
                {
                    Brightness = b
                });
            }
            else
            {
                entities.LiminalCeiling.TurnOff();
            }
        });
    }
}
#endif