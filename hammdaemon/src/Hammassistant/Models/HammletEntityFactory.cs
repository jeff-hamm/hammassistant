using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hammlet.NetDaemon.Models;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Models;
public class HammletEntityFactory : GeneratedEntityFactory
{
    public override Entity CreateEntity(IHaContext haContext, string entityId)
    {
        return base.CreateEntity(haContext, entityId);
    }
}
