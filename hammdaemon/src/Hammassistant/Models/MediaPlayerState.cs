using Hammlet.Extensions;
using Hammlet.NetDaemon.Extensions;
using Hammlet.NetDaemon.Models;

namespace NetDaemon.HassModel.Entities;

public partial interface ITestInterface
{
    public int Value { get; }
}

public partial record MediaPlayerState(EntityState s) : EntityState<MediaPlayerAttributes>(s)
{
    public static MediaPlayerState? FromState(EntityState<MediaPlayerAttributes>? state) => state != null ? new MediaPlayerState(state) : null;
    public static MediaPlayerState? FromState(EntityState? state) => state != null ? new MediaPlayerState(new EntityState<MediaPlayerAttributes>(state)) : null;
    public new MediaPlayerAttributes? Attributes => base.Attributes;


    public new MediaPlayerStates State =>  this.ParseState<MediaPlayerStates>() ?? MediaPlayerStates.Unknown;
}