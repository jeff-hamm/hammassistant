using Vogen;

namespace Hammlet.Extensions;
public enum HaStates : int
{
    Unknown,
    Unavailable,
    On,
    Off
}

public enum MediaPlayerStates : int
{
    Unknown = HaStates.Unknown,
    Unavailable = HaStates.Unavailable,
    On = HaStates.On,
    Off = HaStates.Off,
    Idle,
    Playing,
    Paused,
    Standby,
    Buffering
}

[ValueObject(typeof(string))]
public partial class HaState : IEnumValue<HaStates>
{
    static Validation Validate(string value) => IEnumValue<HaStates>.Validate(value);

    public bool Equals(HaStates other) => this.Equals(other.ToString());
}

public interface IEnumValue<TEnum> : IEquatable<TEnum>
    where TEnum : struct,Enum
{
    public TEnum? EnumValue => EnumExt.TryParse<TEnum>(Value);

    string? Value { get; }

    static Validation Validate(string value) => EnumExt.Validate<TEnum>(value);
}

