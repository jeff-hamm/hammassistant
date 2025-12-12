using System.Text.Json.Serialization;
// export type HassEntityBase = {
//   entity_id: string;
//   state: string;
//   last_changed: string;
//   last_updated: string;
//   attributes: HassEntityAttributeBase;
//   context: Context;
// };// export type HassEntityAttributeBase = {
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
// };// export type HassEntity = HassEntityBase & {
//   attributes: { [key: string]: any };
// };// export type HassEntities = { [entity_id: string]: HassEntity };// export type HassService = {
//   name?: string;
//   description: string;
//   target?: {} | null;
//   fields: {
//     [field_name: string]: {
//       example?: string | boolean | number;
//       default?: unknown;
//       required?: boolean;
//       advanced?: boolean;
//       selector?: {};
//       filter?: {
//         supported_features?: number[];
//         attribute?: Record<string, any[]>;
//       };
//       // Custom integrations don't use translations and still have name/description
//       name?: string;
//       description: string;
//     };
//   };
//   response?: { optional: boolean };
// };
// export type HassUser = {
//   id: string;
//   is_admin: boolean;
//   is_owner: boolean;
//   name: string;
// };// export type HassServiceTarget = {
//   entity_id?: string | string[];
//   device_id?: string | string[];
//   area_id?: string | string[];
//   floor_id?: string | string[];
//   label_id?: string | string[];
// };
namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class HassEventBase
{
    [JsonPropertyName("origin")]
    public string Origin { get; set; }

    [JsonPropertyName("time_fired")]
    public string TimeFired { get; set; }

    [JsonPropertyName("context")]
    public Context Context { get; set; }
}