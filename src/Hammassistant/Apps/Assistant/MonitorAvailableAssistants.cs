using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hammlet.NetDaemon.Models;
using NetDaemon.Extensions.Scheduler;

namespace Hammlet.Apps.Assistant;

[NetDaemonApp]

internal class MonitorAvailableAssistants
{
    public MonitorAvailableAssistants(SelectEntities selects, INetDaemonScheduler scheduler, IAppConfig<AssistantConfig> config)
    {
        scheduler.RunEvery(TimeSpan.FromSeconds(config.Value.AssistantPingTimeSeconds),
            () =>
            {
//                selects.HeyNabuAssistant.Attributes.Options.SelectOption();
            }
        );
    }
}

internal class AssistantConfig
{
    public int AssistantPingTimeSeconds { get; set; } = 60;
}
