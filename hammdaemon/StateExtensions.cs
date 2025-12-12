using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using NetDaemon.HassModel.Entities;

namespace Hammlet.NetDaemon.Extensions;

public static class StateExtensions
{
    public static string ToSnakeCase(this string str) =>
        JsonNamingPolicy.SnakeCaseLower.ConvertName(str);
    public static bool StateChangedTo<TEntity, TEntityState,TAttributes>(this StateChange<TEntity, TEntityState> @this, TEntityState state) 
        where TEntity : Entity<TEntity, EntityState<TAttributes>, TAttributes> 
        where TEntityState : EntityState<TAttributes>
        where TAttributes : class
    {
        return @this.New?.State == state.State;
    }

    public static IObservable<StateChange> StateAndAttributeChanges(this Entity @this) => @this.StateAllChanges();

    public static IEnumerable<TEnum> ParseStates<TEnum>(this IEnumerable<string> states) where TEnum : struct, Enum =>
        states.Select(ParseState<TEnum>).Where(s => s != null).Select(s => s!.Value);

    public static TState? ParseState<TState>(this Entity? @this) where TState : struct, Enum =>
        @this?.EntityState?.ParseState<TState>();
    public static TState? ParseState<TState>(this EntityState? @this) where TState:struct,Enum => Enum.TryParse<TState>(@this?.State, true, out var state) ? state : null;
    public static TState? ParseState<TState>(this string? @this) where TState:struct,Enum => Enum.TryParse<TState>(@this, true, out var state) ? state : null;

    //public static bool IsPlaying([NotNullWhen(true)] this IMediaPlayerEntityCore? entityState) =>
    //    entityState.IsState(MediaPlayerState.Playing);
    //public static bool IsIdle([NotNullWhen(true)] this IMediaPlayerEntityCore? entityState) =>
    //    entityState.IsState(MediaPlayerState.Idle);

    //public static bool IsState<TState>([NotNullWhen(true)] this IEntityCore? entityState, ? value) => entityState != null ?
    //        entityState.HaContext.GetState(entityState.EntityId)
    //        entityState?.State, value?.ToString(), StringComparison.OrdinalIgnoreCase);
    //string.Equals(
    //        entityState.HaContext.GetState(entityState.EntityId)
    //        entityState?.State, value?.ToString(), StringComparison.OrdinalIgnoreCase);
}
