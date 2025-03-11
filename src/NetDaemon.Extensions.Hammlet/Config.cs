﻿using Destructurama;
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
        var argString = $"--{ConfigEnvArg}=";
        if (args.Where(a => a.StartsWith(argString)).Select(a => a.Substring(argString.Length))
                .FirstOrDefault(File.Exists) is not { } envFile)
            if (!EnvReader.TryGetStringValue(ConfigEnvPathVariable, out envFile))
                envFile = Directory.GetCurrentDirectory() + "/.env";

        if(File.Exists(envFile ))
            DotEnv.Fluent().WithEnvFiles(envFile).Load();
        if (!EnvReader.TryGetStringValue(AppConfigFolderVariable, out var appPath))
        {
            if (Path.GetDirectoryName(envFile) is {} envPath &&
                new DirectoryInfo(envPath) is { Exists: true } i && i.EnumerateFiles().Any(f => f.Extension is ".yaml" or ".yml"))
                System.Environment.SetEnvironmentVariable(AppConfigFolderVariable, envPath);
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