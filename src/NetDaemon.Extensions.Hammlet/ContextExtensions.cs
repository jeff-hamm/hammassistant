using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;

namespace HassModel;



public static class ContextExtensions
{

    public static IEnumerable<TEntity> Entities<TEntity>(this IHaContext @this, IEnumerable<string> entityIds) 
        where TEntity : Entity
        => entityIds.Select(@this.Entity<TEntity>);

    public static TEntity Entity<TEntity>(this IHaContext @this, string entityId) where TEntity : Entity =>
        (TEntity)Activator.CreateInstance(typeof(TEntity), @this, entityId)!;
}

