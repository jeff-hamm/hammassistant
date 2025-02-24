using System.Text.Json;
using Hammlet.Extensions;
using Hammlet.NetDaemon.Models;
using NetDaemon.HassModel.Entities;

namespace Hammlet.NetDaemon.apps.Media;
#if HAMMLET
/// <summary>
///     Showcase using the new HassModel API and turn on light on movement
/// </summary>
[NetDaemonApp]
public class OnYoutubeMusicPlaylistChanged
{
    public OnYoutubeMusicPlaylistChanged(IHaContext ha, SelectEntities entities, MediaPlayerEntities mediaPLayers, SensorEntities sensors, ILogger<OnYoutubeMusicPlaylistChanged> logger)
    {
        mediaPLayers.HammletOledTv.StateChanges().Subscribe(e =>
        {
            if (e.Old?.State == MediaPlayerStates.On || e.New?.State != MediaPlayerStates.On) return;
            EnsureHammletCast(mediaPLayers, logger);
        });
        mediaPLayers.YtubeMusicPlayer.StateChanges().Subscribe(e =>
        {
            if (e.Old?.State == MediaPlayerStates.Playing || e.New?.State != MediaPlayerStates.Playing) return;
            EnsureHammletCast(mediaPLayers, logger);
        });
        entities.YtubeMusicPlayerPlaylist.StateChanges().Subscribe(e =>
        {
            if (e.New?.State is not {} playListName)
            {
                if (e.Old?.State == null) return;
                if (!mediaPLayers.YtubeMusicPlayer.IsOn()) return;
                mediaPLayers.YtubeMusicPlayer.MediaStop();
                logger.LogDebug("Changed playlist to empty, stopping playlist");
                return;
            }
            var pl = (object?)sensors.YtubeMusicPlayerExtra.Attributes?.Playlists;
            if (pl is not JsonElement { ValueKind: JsonValueKind.Object } obj)
            {
                logger.LogError("Unexpected playlist attributes type {playlistType}",pl?.GetType()?.ToString() ?? "Null");
                return;
            }
            if (!obj.TryGetProperty(playListName, out var propertyValue))
            {
                logger.LogError("Could not find playlist {playList} in YtubeMusicPlayerExtra", playListName);
                return;
            } 

            if (propertyValue.ValueKind != JsonValueKind.String || propertyValue.GetString() is not {} val) 
            {
                logger.LogError("Unexpected property ValueKind {ValueKind} {Value} in YtubeMusicPlayerExtra", propertyValue.ValueKind, propertyValue);
                return;

            }
            mediaPLayers.YtubeMusicPlayer.PlayMedia(val,"playlist");
        });
    }

    private static void EnsureHammletCast(MediaPlayerEntities mediaPLayers, ILogger<OnYoutubeMusicPlaylistChanged> logger)
    {
        if (!mediaPLayers.HammletCast.IsOff()) return;
        logger.LogDebug("Turning on HammletCast just turned on");
        mediaPLayers.HammletCast.TurnOn();
    }
}

#endif