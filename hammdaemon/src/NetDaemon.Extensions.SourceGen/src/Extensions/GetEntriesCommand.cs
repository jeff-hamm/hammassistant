using System.Text.Json.Serialization;
using NetDaemon.Client.HomeAssistant.Model;

namespace NetDaemon.Extensions.SourceGen.src.Extensions;

internal record GetEntriesCommand : CommandMessage
{
    public GetEntriesCommand(string type)
    {
        Type = type;
    }
    [JsonPropertyName("entity_ids")] public required IReadOnlyList<string> EntityIds { get; init; }
}