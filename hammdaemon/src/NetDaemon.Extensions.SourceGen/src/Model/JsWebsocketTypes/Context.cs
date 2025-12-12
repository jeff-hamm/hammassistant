using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;
// export type Error = 1 | 2 | 3 | 4;

// export type UnsubscribeFunc = () => void;

// export type Context = {
//   id: string;
//   user_id: string | null;
//   parent_id: string | null;
// };
public class Context
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("parent_id")]
    public string ParentId { get; set; }
}

// export type HassEventBase = {
//   origin: string;
//   time_fired: string;
//   context: Context;
// };

// export type HassEvent = HassEventBase & {
//   event_type: string;
//   data: { [key: string]: any };
// };

// export type StateChangedEvent = HassEventBase & {
//   event_type: "state_changed";
//   data: {
//     entity_id: string;
//     new_state: HassEntity | null;
//     old_state: HassEntity | null;
//   };
// };

// export type HassConfig = {
//   latitude: number;
//   longitude: number;
//   elevation: number;
//   radius: number;
//   unit_system: {
//     length: string;
//     mass: string;
//     volume: string;
//     temperature: string;
//     pressure: string;
//     wind_speed: string;
//     accumulated_precipitation: string;
//   };
//   location_name: string;
//   time_zone: string;
//   components: string[];
//   config_dir: string;
//   allowlist_external_dirs: string[];
//   allowlist_external_urls: string[];
//   version: string;
//   config_source: string;
//   recovery_mode: boolean;
//   safe_mode: boolean;
//   state: "NOT_RUNNING" | "STARTING" | "RUNNING" | "STOPPING" | "FINAL_WRITE";
//   external_url: string | null;
//   internal_url: string | null;
//   currency: string;
//   country: string | null;
//   language: string;
// };

// export type HassEntityBase = {
//   entity_id: string;
//   state: string;
//   last_changed: string;
//   last_updated: string;
//   attributes: HassEntityAttributeBase;
//   context: Context;
// };

// export type HassEntityAttributeBase = {
//   friendly_name?: string;
//   unit_of_measurement?: string;
//   icon?: string;
//   entity_picture?: string;
//   supported_features?: number;
//   hidden?: boolean;
//   assumed_state?: boolean;
//   device_class?: string;
//   state_class?: string;
//   restored?: boolean;
// };

// export type HassEntity = HassEntityBase & {
//   attributes: { [key: string]: any };
// };


