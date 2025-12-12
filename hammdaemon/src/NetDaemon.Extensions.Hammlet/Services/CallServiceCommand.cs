using System.Text;
using System.Text.Json.Serialization;
using NetDaemon.Client.HomeAssistant.Model;
using NetDaemon.HassModel.Entities;

namespace NetDaemon.Extensions.Hammlet.Services;
public static class ServiceExtensions
{
    public static EntityStateEventMonitor ToStateMonitor(this IEntityCore entity, Func<EntityState, bool> found)
    {
        return new EntityStateEventMonitor(entity.EntityId, evt => found(evt.State));
    }
    public static ServiceCall ToServiceCall(this IEntityCore entity, string service, object? data = null)
    {
        ArgumentNullException.ThrowIfNull((object) entity, nameof (entity));
        ArgumentNullException.ThrowIfNull((object) service, nameof (service));
        return new ServiceCall(
            EntityId.From(entity.EntityId).Domain,
            service, new HassTarget()
        {
            EntityIds = [entity.EntityId]
        }, data);
    }

}


internal record CallServiceCommand : CommandMessage
{
    public CallServiceCommand() => Type = "call_service";

    [JsonPropertyName("domain")]
    public string Domain { get; init; } = string.Empty;

    [JsonPropertyName("service")]
    public string Service { get; init; } = string.Empty;

    [JsonPropertyName("service_data")]
    public object? ServiceData { get; init; }

    [JsonPropertyName("target")]
    public HassTarget? Target { get; init; }

    protected override bool PrintMembers(StringBuilder builder)
    {
        if (base.PrintMembers(builder))
            builder.Append(", ");
        builder.Append("Domain = ");
        builder.Append((object)Domain);
        builder.Append(", Service = ");
        builder.Append((object)Service);
        builder.Append(", ServiceData = ");
        builder.Append(ServiceData);
        builder.Append(", Target = ");
        builder.Append(Target);
        return true;
    }
}