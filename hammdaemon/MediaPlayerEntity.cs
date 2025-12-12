using Hammlet.Extensions;
using Hammlet.Models.Enums;
using Hammlet.NetDaemon.Extensions;
using NetDaemon.HassModel.Entities;

namespace Hammlet.NetDaemon.Models;


public partial record MediaPlayerEntity
{
    public new MediaPlayerState? EntityState => base.EntityState != null ? (MediaPlayerState)base.EntityState : null;
    public new MediaPlayerStates State =>
        base.EntityState?.ParseState<MediaPlayerStates>() ?? MediaPlayerStates.Unknown;

//    public new MediaPlayerState? EntityState => new MediaPlayerState(base.EntityState);

    public new IObservable<StateChange<MediaPlayerEntity, MediaPlayerState>> StateChanges() =>
        ((Entity)this).StateChanges().Select(c => new StateChange<MediaPlayerEntity, MediaPlayerState>(
            (MediaPlayerEntity)c.Entity,
            MediaPlayerState.FromState(c.Old), MediaPlayerState.FromState(c.New)));
}
