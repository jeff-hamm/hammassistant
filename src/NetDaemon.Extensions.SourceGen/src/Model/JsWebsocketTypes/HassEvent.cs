using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;
// export type HassEvent = HassEventBase & {
//   event_type: string;
//   data: { [key: string]: any };
// };// export type StateChangedEvent = HassEventBase & {
//   event_type: "state_changed";
//   data: {
//     entity_id: string;
//     new_state: HassEntity | null;
//     old_state: HassEntity | null;
//   };
// };
public class HassEvent : HassEventBase
{
    [JsonPropertyName("event_type")]
    public string EventType { get; set; }

    [JsonPropertyName("data")]
    public Dictionary<string, JsonAny> Data { get; set; }
}