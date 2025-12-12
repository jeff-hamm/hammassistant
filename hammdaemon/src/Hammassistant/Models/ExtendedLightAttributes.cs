using System.Text.Json.Serialization;
using NetDaemon.HassModel.Entities;

namespace Hammlet.NetDaemon.Models;


public interface IStateEntity<out TState> where TState : IParsable<TState>
{
    public EntityState? EntityState { get; }
}
public partial record LightAttributes
{
    [JsonIgnore] public ExtendedLightAttributes? Extended { get; init; } = new ExtendedLightAttributes();
    public object? RgbwColor { get; set; }
}

public partial class ExtendedLightAttributes
{

}
