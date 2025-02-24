using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

public static class EnumExtensions
{
    private static readonly ConcurrentDictionary<Enum, string> _cache = new();

    public static string ToJsonName(this Enum enumValue)
    {
        return _cache.GetOrAdd(enumValue, e =>
        {
            var type = e.GetType();
            var memberInfo = type.GetMember(e.ToString());
            var attribute = memberInfo[0].GetCustomAttribute<JsonStringEnumMemberNameAttribute>();
            return attribute?.Name ?? e.ToString();
        });
    }
}
