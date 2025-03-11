using Destructurama;
using dotenv.net;
using dotenv.net.Utilities;
using Hammlet.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetDaemon.AppModel;
using NetDaemon.Extensions.Hammlet.Model;
using NetDaemon.Extensions.Hammlet.Services;
using NetDaemon.Extensions.Logging;
using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Compact;
using Vogen;

[assembly: Vogen.VogenDefaults(
    conversions: Conversions.TypeConverter | Conversions.SystemTextJson,
    throws: typeof(ValueObjectValidationException))]


namespace NetDaemon.Extensions.Hammlet;

public static class Config
{
    public const string ConfigEnvPathVariable = "NetDaemon__EnvFile";
    public const string AppConfigFolderVariable = $"NetDaemon__{nameof(AppConfigurationLocationSetting.ApplicationConfigurationFolder)}";
    public const string ConfigEnvArg = "env";
    public static IHostBuilder UseTheHammlet(this IHostBuilder @this, params string[] args)
    {
        var logger = Log.Logger;
        var argString = $"--{ConfigEnvArg}=";
        if (args.Where(a => a.StartsWith(argString))
                .Select(a => a.Substring(argString.Length))
                .FirstOrDefault(File.Exists) is not { } envFile)
        {
            if (!EnvReader.TryGetStringValue(ConfigEnvPathVariable, out envFile))
            {
                envFile = Directory.GetCurrentDirectory() + "/.env";
            }
            else
            {
                logger.Debug("Found {envFile} from environment variable {EnvName}",envFile,ConfigEnvPathVariable);
            }
        }
        else
        {
            logger.Debug("Found {envFile} from args parameter --{ParamName}",envFile,ConfigEnvArg);
        }

        if (File.Exists(envFile))
        {
            logger.Debug("Env file {envFile} exists, loading it", envFile);
            DotEnv.Fluent().WithEnvFiles(envFile).WithOverwriteExistingVars().Load();
        }
        else
        {
            logger.Debug("Env file {envFile} does not exist, skipping loading", envFile);
        }

        if (!EnvReader.TryGetStringValue(AppConfigFolderVariable, out var appPath))
        {
            if (Path.GetDirectoryName(envFile) is { } envPath &&
                Directory.Exists(envPath))
            {
                
                if (Directory.EnumerateFiles(envPath, "*.y*", SearchOption.AllDirectories).Any())
                {
                    logger.Debug("Found {appPath} from envPath {EnvPath}", appPath, envPath);
                    System.Environment.SetEnvironmentVariable(AppConfigFolderVariable, envPath);
                }
                else
                {
                    logger.Debug("No yaml files found in {EnvPath}, skipping setting {AppConfigFolderVariable}", envPath, AppConfigFolderVariable);
                }
            }
        }
        else
        {
            logger.Debug("Found {appPath} from environment variable {EnvName}", appPath, AppConfigFolderVariable);
        }
        @this.ConfigureHostConfiguration(cb => cb.AddEnvironmentVariables());
        @this.ConfigureAppConfiguration(cb => cb.AddEnvironmentVariables());
        return @this.UseTheHammletLogging();
    }

    public static IHostBuilder UseTheHammletLogging(this IHostBuilder @this)
    {
        @this.UseNetDaemonDefaultLogging()
            .UseSerilog(((c, sp, cfg) =>
            {
                cfg.ReadFrom.Configuration(c.Configuration)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning).MinimumLevel
                    .Override("System.Net.Http.HttpClient", LogEventLevel.Warning).Enrich.FromLogContext()
                    .Destructure.ByIgnoringPropertiesOfTypeAssignableTo<IHaContext>()
                    .Destructure.ByIgnoring<EntityState>(o => o.Ignore(s => s.AttributesJson).Ignore(s => s.Context)
                    );
                var staticLogger = new LoggerProviderCollection();
                foreach (var lsp in sp.GetServices<ILoggerProvider>())
                    staticLogger.AddProvider(lsp);

            }));
        return @this;
    }
    public static IServiceCollection AddTheHammlet(this IServiceCollection @this)
    {
        @this.AddScoped<AsyncServiceCaller>();
        @this.AddScoped<IAsyncHaContext, AsyncHaContext>();
        @this.AddTransient(typeof(IAppConfig<>), typeof(HammassistantAppConfig<>));
        return @this;

    }
}