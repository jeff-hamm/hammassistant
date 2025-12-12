using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

// export type HassUser = {
//   id: string;
//   is_admin: boolean;
//   is_owner: boolean;
//   name: string;
// };
public class HassUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("is_admin")]
    public bool IsAdmin { get; set; }

    [JsonPropertyName("is_owner")]
    public bool IsOwner { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}