using NetDaemon.Client;
using NetDaemon.Extensions.SourceGen.Model.FrontEndTypes;
using NetDaemon.Extensions.SourceGen.src.Extensions;

namespace NetDaemon.Extensions.SourceGen.Extensions;

public static class HomeAssistantConnectionExtensions
{
    public static async Task<Dictionary<string,ExtEntityRegistryEntry>?> GetExtendedEntitiesAsync(
        this IHomeAssistantConnection connection,
        string[] entityIds, CancellationToken cancelToken)
    {
        return await connection
            .SendCommandAndReturnResponseAsync<GetEntriesCommand,
                Dictionary<string,ExtEntityRegistryEntry>>(new GetEntriesCommand("config/entity_registry/get_entries")
            {
                EntityIds = entityIds
            }, cancelToken).ConfigureAwait(false);
    }
}
