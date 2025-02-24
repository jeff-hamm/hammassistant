using Hammlet.NetDaemon.Models;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Apps.Lights;
#if HAMMLET
[NetDaemonApp]
internal class SetToWarmWhite
{
    public SetToWarmWhite(IHaContext ha, ConversationEntities entities,BinarySensorEntities buttons, EventEntities events, ILogger<LiminalDimmer> logger)
    {
        buttons.LiminalDimmerInput0.StateChanges().Subscribe(s =>
        {
            logger.LogInformation("1 Input: {0}", s.New?.State);
        });
        buttons.LiminalDimmerInput1.StateChanges().Subscribe(s =>
        {
            logger.LogInformation("2 Input: {0}", s.New?.State);
        });
        entities.LiminalDimmer.StateAllCLhanges().Subscribe(s =>
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
