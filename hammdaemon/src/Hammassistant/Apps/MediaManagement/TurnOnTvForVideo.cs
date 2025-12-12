// Use unique namespaces for your apps if you going to share with others to avoid
// conflicting names

using System.Text.Json;
using Hammlet.Extensions;
using Hammlet.NetDaemon.Models;
using NetDaemon.HassModel.Entities;

namespace Hammlet.NetDaemon.apps.MediaManagement;
#if HAMMLET
/// <summary>
///     Showcase using the new HassModel API and turn on light on movement
/// </summary>
[NetDaemonApp]
public class TurnOnTvForVideo
{
    private static readonly JsonSerializerOptions o = new()
    {
        WriteIndented = true,
    };
    public TurnOnTvForVideo(IHaContext ha, MediaPlayerEntities entities)
    {
        bool hasBeenOff = entities.HammletCast.IsOff();
        entities.HammletCast.StateChanges()
            .Where(s => 
                s.New?.State == MediaPlayerStates.Off ||
                (s.New?.State == MediaPlayerStates.Playing && s.Old?.State != MediaPlayerStates.Playing))
            .Subscribe(e =>
        {
            if (e.New?.State == MediaPlayerStates.Off)
            {
                hasBeenOff = true;
                return;
            }
            if(hasBeenOff &&
                entities.HammletOledTv.State == MediaPlayerStates.Off &&
                e.New?.Attributes?.MediaContentType?.Equals("video", StringComparison.CurrentCultureIgnoreCase) == true)
            {
                entities.HammletOledTv.TurnOn();
            }
        });
    }
}
#endif