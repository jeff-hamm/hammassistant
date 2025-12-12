using Vogen;

namespace Hammlet.Extensions;

public static class EnumExt
{
    public static TEnum? TryParse<TEnum>(string? value) where TEnum : struct, Enum => Enum.TryParse<TEnum>(value, true, out var v) ? v : default;
    public static Validation Validate<TEnum>(string? value) where TEnum : struct, Enum =>
        Enum.TryParse<TEnum>(value, out var v) ? Validation.Ok : Validation.Invalid($"Invalid {typeof(TEnum)} value {value}");

}