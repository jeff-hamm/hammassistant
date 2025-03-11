using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetDaemon.AppModel;
using NetDaemon.AppModel.Internal.Config;

namespace NetDaemon.Extensions.Hammlet;

internal class HammassistantAppConfig<T> : IAppConfig<T> where T : class, new()
{
    public HammassistantAppConfig(IConfiguration config, IServiceProvider provider, ILogger<HammassistantAppConfig<T>> logger)
    {
        var type = typeof(T);
        var section = config.GetSection(type.FullName!);
        if (!section.Exists())
        {
            logger.LogDebug("The configuration for {Type} could not be found, looking for {TypeName}.", typeof(T).FullName, type.Name);
            section = config.GetSection(type.Name);
        }

        if (!section.Exists() && TryTrimmed(out var name))
        {
            logger.LogDebug("The configuration for {Type} could not be found, looking for {TypeName}.",
                typeof(T).FullName, name);
            section = config.GetSection(name);
        }

        if(!section.Exists())
        {
            logger.LogWarning("The configuration for {Type} is not found. Please add config.", typeof(T).FullName);
            Value = new T();
        }
        else
        {
            Value = config.Get<T>(provider) ?? new T();
        }
    }

    private static bool TryTrimmed(out string name)
    {
        name = typeof(T).Name;
        if(name.EndsWith("Config")) name = name[..^("Config".Length)];
        else if (name.EndsWith("Options")) name = name[..^("Options".Length)];
        else return false;
        return true;
    }

    public T Value { get; }
}