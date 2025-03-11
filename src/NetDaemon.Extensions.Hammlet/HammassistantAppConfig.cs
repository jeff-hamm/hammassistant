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
            logger.LogDebug("The configuration for {Type} could not be found, looking for {TypeName}.", typeof(T).FullName, typeof(T).Name);
            section = config.GetSection(type.Name);
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

    public T Value { get; }
}