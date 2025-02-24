using System.Collections.Generic;

namespace Hammlet.NetDaemon.Models;
#if HAMMLET
public partial class EventEntities
{
    public IEnumerable<EventEntity> AliveRemoteRemoteControl =>
        [AliveRemoteScene001, AliveRemoteScene002, AliveRemoteScene003, AliveRemoteScene004];
}
#endif