//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.2015676-08:00
//
// *** Make sure the version of the codegen tool and your nugets NetDaemon.* have the same version.***
// You can use following command to keep it up to date with the latest version:
//   dotnet tool update NetDaemon.HassModel.CodeGen
//
// To update this file with latest entities run this command in your project directory:
//   dotnet tool run nd-codegen
//
// In the template projects we provided a convenience powershell script that will update
// the codegen and nugets to latest versions update_all_dependencies.ps1.
//
// For more information: https://netdaemon.xyz/docs/user/hass_model/hass_model_codegen
// For more information about NetDaemon: https://netdaemon.xyz/
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;
using NetDaemon.HassModel.Entities.Core;

namespace Hammlet.NetDaemon.Models;
public static class MediaPlayerEntityExtensionMethods
{
    ///<summary>Removes all items from the playlist.</summary>
    public static void ClearPlaylist(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("clear_playlist", data);
    }

    ///<summary>Removes all items from the playlist.</summary>
    public static void ClearPlaylist(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("clear_playlist", data);
    }

    ///<summary>Groups media players together for synchronous playback. Only works on supported multiroom audio systems.</summary>
    public static void Join(this IMediaPlayerEntityCore target, MediaPlayerJoinParameters data)
    {
        target.CallService("join", data);
    }

    ///<summary>Groups media players together for synchronous playback. Only works on supported multiroom audio systems.</summary>
    public static void Join(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerJoinParameters data)
    {
        target.CallService("join", data);
    }

    ///<summary>Groups media players together for synchronous playback. Only works on supported multiroom audio systems.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="groupMembers">The players which will be synced with the playback specified in &apos;Targets&apos;. eg: - media_player.multiroom_player2 - media_player.multiroom_player3 </param>
    public static void Join(this IMediaPlayerEntityCore target, IEnumerable<string> groupMembers)
    {
        target.CallService("join", new MediaPlayerJoinParameters { GroupMembers = groupMembers });
    }

    ///<summary>Groups media players together for synchronous playback. Only works on supported multiroom audio systems.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="groupMembers">The players which will be synced with the playback specified in &apos;Targets&apos;. eg: - media_player.multiroom_player2 - media_player.multiroom_player3 </param>
    public static void Join(this IEnumerable<IMediaPlayerEntityCore> target, IEnumerable<string> groupMembers)
    {
        target.CallService("join", new MediaPlayerJoinParameters { GroupMembers = groupMembers });
    }

    ///<summary>Selects the next track.</summary>
    public static void MediaNextTrack(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("media_next_track", data);
    }

    ///<summary>Selects the next track.</summary>
    public static void MediaNextTrack(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("media_next_track", data);
    }

    ///<summary>Pauses.</summary>
    public static void MediaPause(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("media_pause", data);
    }

    ///<summary>Pauses.</summary>
    public static void MediaPause(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("media_pause", data);
    }

    ///<summary>Starts playing.</summary>
    public static void MediaPlay(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("media_play", data);
    }

    ///<summary>Starts playing.</summary>
    public static void MediaPlay(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("media_play", data);
    }

    ///<summary>Toggles play/pause.</summary>
    public static void MediaPlayPause(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("media_play_pause", data);
    }

    ///<summary>Toggles play/pause.</summary>
    public static void MediaPlayPause(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("media_play_pause", data);
    }

    ///<summary>Selects the previous track.</summary>
    public static void MediaPreviousTrack(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("media_previous_track", data);
    }

    ///<summary>Selects the previous track.</summary>
    public static void MediaPreviousTrack(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("media_previous_track", data);
    }

    ///<summary>Allows you to go to a different part of the media that is currently playing.</summary>
    public static void MediaSeek(this IMediaPlayerEntityCore target, MediaPlayerMediaSeekParameters data)
    {
        target.CallService("media_seek", data);
    }

    ///<summary>Allows you to go to a different part of the media that is currently playing.</summary>
    public static void MediaSeek(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerMediaSeekParameters data)
    {
        target.CallService("media_seek", data);
    }

    ///<summary>Allows you to go to a different part of the media that is currently playing.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="seekPosition">Target position in the currently playing media. The format is platform dependent.</param>
    public static void MediaSeek(this IMediaPlayerEntityCore target, double seekPosition)
    {
        target.CallService("media_seek", new MediaPlayerMediaSeekParameters { SeekPosition = seekPosition });
    }

    ///<summary>Allows you to go to a different part of the media that is currently playing.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="seekPosition">Target position in the currently playing media. The format is platform dependent.</param>
    public static void MediaSeek(this IEnumerable<IMediaPlayerEntityCore> target, double seekPosition)
    {
        target.CallService("media_seek", new MediaPlayerMediaSeekParameters { SeekPosition = seekPosition });
    }

    ///<summary>Stops playing.</summary>
    public static void MediaStop(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("media_stop", data);
    }

    ///<summary>Stops playing.</summary>
    public static void MediaStop(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("media_stop", data);
    }

    ///<summary>Starts playing specified media.</summary>
    public static void PlayMedia(this IMediaPlayerEntityCore target, MediaPlayerPlayMediaParameters data)
    {
        target.CallService("play_media", data);
    }

    ///<summary>Starts playing specified media.</summary>
    public static void PlayMedia(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerPlayMediaParameters data)
    {
        target.CallService("play_media", data);
    }

    ///<summary>Starts playing specified media.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="mediaContentId">The ID of the content to play. Platform dependent. eg: https://home-assistant.io/images/cast/splash.png</param>
    ///<param name="mediaContentType">The type of the content to play. Such as image, music, tv show, video, episode, channel, or playlist. eg: music</param>
    ///<param name="enqueue">If the content should be played now or be added to the queue.</param>
    ///<param name="announce">If the media should be played as an announcement. eg: true</param>
    public static void PlayMedia(this IMediaPlayerEntityCore target, string mediaContentId, string mediaContentType, object? enqueue = null, bool? announce = null)
    {
        target.CallService("play_media", new MediaPlayerPlayMediaParameters { MediaContentId = mediaContentId, MediaContentType = mediaContentType, Enqueue = enqueue, Announce = announce });
    }

    ///<summary>Starts playing specified media.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="mediaContentId">The ID of the content to play. Platform dependent. eg: https://home-assistant.io/images/cast/splash.png</param>
    ///<param name="mediaContentType">The type of the content to play. Such as image, music, tv show, video, episode, channel, or playlist. eg: music</param>
    ///<param name="enqueue">If the content should be played now or be added to the queue.</param>
    ///<param name="announce">If the media should be played as an announcement. eg: true</param>
    public static void PlayMedia(this IEnumerable<IMediaPlayerEntityCore> target, string mediaContentId, string mediaContentType, object? enqueue = null, bool? announce = null)
    {
        target.CallService("play_media", new MediaPlayerPlayMediaParameters { MediaContentId = mediaContentId, MediaContentType = mediaContentType, Enqueue = enqueue, Announce = announce });
    }

    ///<summary>Playback mode that plays the media in a loop.</summary>
    public static void RepeatSet(this IMediaPlayerEntityCore target, MediaPlayerRepeatSetParameters data)
    {
        target.CallService("repeat_set", data);
    }

    ///<summary>Playback mode that plays the media in a loop.</summary>
    public static void RepeatSet(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerRepeatSetParameters data)
    {
        target.CallService("repeat_set", data);
    }

    ///<summary>Playback mode that plays the media in a loop.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="repeat">Repeat mode to set.</param>
    public static void RepeatSet(this IMediaPlayerEntityCore target, object repeat)
    {
        target.CallService("repeat_set", new MediaPlayerRepeatSetParameters { Repeat = repeat });
    }

    ///<summary>Playback mode that plays the media in a loop.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="repeat">Repeat mode to set.</param>
    public static void RepeatSet(this IEnumerable<IMediaPlayerEntityCore> target, object repeat)
    {
        target.CallService("repeat_set", new MediaPlayerRepeatSetParameters { Repeat = repeat });
    }

    ///<summary>Selects a specific sound mode.</summary>
    public static void SelectSoundMode(this IMediaPlayerEntityCore target, MediaPlayerSelectSoundModeParameters data)
    {
        target.CallService("select_sound_mode", data);
    }

    ///<summary>Selects a specific sound mode.</summary>
    public static void SelectSoundMode(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerSelectSoundModeParameters data)
    {
        target.CallService("select_sound_mode", data);
    }

    ///<summary>Selects a specific sound mode.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="soundMode">Name of the sound mode to switch to. eg: Music</param>
    public static void SelectSoundMode(this IMediaPlayerEntityCore target, string? soundMode = null)
    {
        target.CallService("select_sound_mode", new MediaPlayerSelectSoundModeParameters { SoundMode = soundMode });
    }

    ///<summary>Selects a specific sound mode.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="soundMode">Name of the sound mode to switch to. eg: Music</param>
    public static void SelectSoundMode(this IEnumerable<IMediaPlayerEntityCore> target, string? soundMode = null)
    {
        target.CallService("select_sound_mode", new MediaPlayerSelectSoundModeParameters { SoundMode = soundMode });
    }

    ///<summary>Sends the media player the command to change input source.</summary>
    public static void SelectSource(this IMediaPlayerEntityCore target, MediaPlayerSelectSourceParameters data)
    {
        target.CallService("select_source", data);
    }

    ///<summary>Sends the media player the command to change input source.</summary>
    public static void SelectSource(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerSelectSourceParameters data)
    {
        target.CallService("select_source", data);
    }

    ///<summary>Sends the media player the command to change input source.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="source">Name of the source to switch to. Platform dependent. eg: video1</param>
    public static void SelectSource(this IMediaPlayerEntityCore target, string source)
    {
        target.CallService("select_source", new MediaPlayerSelectSourceParameters { Source = source });
    }

    ///<summary>Sends the media player the command to change input source.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="source">Name of the source to switch to. Platform dependent. eg: video1</param>
    public static void SelectSource(this IEnumerable<IMediaPlayerEntityCore> target, string source)
    {
        target.CallService("select_source", new MediaPlayerSelectSourceParameters { Source = source });
    }

    ///<summary>Playback mode that selects the media in randomized order.</summary>
    public static void ShuffleSet(this IMediaPlayerEntityCore target, MediaPlayerShuffleSetParameters data)
    {
        target.CallService("shuffle_set", data);
    }

    ///<summary>Playback mode that selects the media in randomized order.</summary>
    public static void ShuffleSet(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerShuffleSetParameters data)
    {
        target.CallService("shuffle_set", data);
    }

    ///<summary>Playback mode that selects the media in randomized order.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="shuffle">Whether or not shuffle mode is enabled.</param>
    public static void ShuffleSet(this IMediaPlayerEntityCore target, bool shuffle)
    {
        target.CallService("shuffle_set", new MediaPlayerShuffleSetParameters { Shuffle = shuffle });
    }

    ///<summary>Playback mode that selects the media in randomized order.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="shuffle">Whether or not shuffle mode is enabled.</param>
    public static void ShuffleSet(this IEnumerable<IMediaPlayerEntityCore> target, bool shuffle)
    {
        target.CallService("shuffle_set", new MediaPlayerShuffleSetParameters { Shuffle = shuffle });
    }

    ///<summary>Toggles a media player on/off.</summary>
    public static void Toggle(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("toggle", data);
    }

    ///<summary>Toggles a media player on/off.</summary>
    public static void Toggle(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("toggle", data);
    }

    ///<summary>Turns off the power of the media player.</summary>
    public static void TurnOff(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("turn_off", data);
    }

    ///<summary>Turns off the power of the media player.</summary>
    public static void TurnOff(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("turn_off", data);
    }

    ///<summary>Turns on the power of the media player.</summary>
    public static void TurnOn(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("turn_on", data);
    }

    ///<summary>Turns on the power of the media player.</summary>
    public static void TurnOn(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("turn_on", data);
    }

    ///<summary>Removes the player from a group. Only works on platforms which support player groups.</summary>
    public static void Unjoin(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("unjoin", data);
    }

    ///<summary>Removes the player from a group. Only works on platforms which support player groups.</summary>
    public static void Unjoin(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("unjoin", data);
    }

    ///<summary>Turns down the volume.</summary>
    public static void VolumeDown(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("volume_down", data);
    }

    ///<summary>Turns down the volume.</summary>
    public static void VolumeDown(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("volume_down", data);
    }

    ///<summary>Mutes or unmutes the media player.</summary>
    public static void VolumeMute(this IMediaPlayerEntityCore target, MediaPlayerVolumeMuteParameters data)
    {
        target.CallService("volume_mute", data);
    }

    ///<summary>Mutes or unmutes the media player.</summary>
    public static void VolumeMute(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerVolumeMuteParameters data)
    {
        target.CallService("volume_mute", data);
    }

    ///<summary>Mutes or unmutes the media player.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="isVolumeMuted">Defines whether or not it is muted.</param>
    public static void VolumeMute(this IMediaPlayerEntityCore target, bool isVolumeMuted)
    {
        target.CallService("volume_mute", new MediaPlayerVolumeMuteParameters { IsVolumeMuted = isVolumeMuted });
    }

    ///<summary>Mutes or unmutes the media player.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="isVolumeMuted">Defines whether or not it is muted.</param>
    public static void VolumeMute(this IEnumerable<IMediaPlayerEntityCore> target, bool isVolumeMuted)
    {
        target.CallService("volume_mute", new MediaPlayerVolumeMuteParameters { IsVolumeMuted = isVolumeMuted });
    }

    ///<summary>Sets the volume level.</summary>
    public static void VolumeSet(this IMediaPlayerEntityCore target, MediaPlayerVolumeSetParameters data)
    {
        target.CallService("volume_set", data);
    }

    ///<summary>Sets the volume level.</summary>
    public static void VolumeSet(this IEnumerable<IMediaPlayerEntityCore> target, MediaPlayerVolumeSetParameters data)
    {
        target.CallService("volume_set", data);
    }

    ///<summary>Sets the volume level.</summary>
    ///<param name="target">The IMediaPlayerEntityCore to call this service for</param>
    ///<param name="volumeLevel">The volume. 0 is inaudible, 1 is the maximum volume.</param>
    public static void VolumeSet(this IMediaPlayerEntityCore target, double volumeLevel)
    {
        target.CallService("volume_set", new MediaPlayerVolumeSetParameters { VolumeLevel = volumeLevel });
    }

    ///<summary>Sets the volume level.</summary>
    ///<param name="target">The IEnumerable&lt;IMediaPlayerEntityCore&gt; to call this service for</param>
    ///<param name="volumeLevel">The volume. 0 is inaudible, 1 is the maximum volume.</param>
    public static void VolumeSet(this IEnumerable<IMediaPlayerEntityCore> target, double volumeLevel)
    {
        target.CallService("volume_set", new MediaPlayerVolumeSetParameters { VolumeLevel = volumeLevel });
    }

    ///<summary>Turns up the volume.</summary>
    public static void VolumeUp(this IMediaPlayerEntityCore target, object? data = null)
    {
        target.CallService("volume_up", data);
    }

    ///<summary>Turns up the volume.</summary>
    public static void VolumeUp(this IEnumerable<IMediaPlayerEntityCore> target, object? data = null)
    {
        target.CallService("volume_up", data);
    }
}