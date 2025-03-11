using System.Reflection;
using Destructurama;
using Hammlet.Extensions;
using Hammlet.NetDaemon.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDaemon.Extensions.Hammlet;
using NetDaemon.Extensions.Logging;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Extensions.Scheduler;
using NetDaemon.Extensions.Tts;
using NetDaemon.HassModel.Entities;
using NetDaemon.Runtime;
using Serilog;

#pragma warning disable CA1812


// Initialize early, without access to configuration or services
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // + file or centralized logging
    .CreateLogger();

try
{
    var host = Host.CreateDefaultBuilder(args)
        .UseTheHammlet()
        .UseNetDaemonAppSettings()
        .UseNetDaemonRuntime()
        .UseNetDaemonMqttEntityManagement()
        .UseNetDaemonTextToSpeech()
        .ConfigureServices((_, services) =>
            services
                .AddAppsFromAssembly(Assembly.GetExecutingAssembly())
                .AddNetDaemonStateManager()
                .AddNetDaemonScheduler()
                .AddHomeAssistantGenerated()
                .AddTheHammlet()
        )
        .Build();
    LogUtil.LoggerFactory= host.Services.GetRequiredService<ILoggerFactory>();
    await host.RunAsync()
        .ConfigureAwait(false);}
catch (Exception ex)
{
    // Any unhandled exception during start-up will be caught and flushed to
    // our log file or centralized log server
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}
